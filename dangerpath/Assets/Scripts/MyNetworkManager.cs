using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class CustomNetworkManager : NetworkManager
{
    private GameRoomManager gameRoomManager;
    public static event Action<List<GameRoom>> OnLobbiesUpdated;
    public static Dictionary<Guid, GameRoom> openMatches = new Dictionary<Guid, GameRoom>();
    public static Dictionary<Guid, HashSet<NetworkConnectionToClient>> matchConnections = new Dictionary<Guid, HashSet<NetworkConnectionToClient>>();
    public static List<NetworkConnectionToClient> waitingConnections = new List<NetworkConnectionToClient>();

    public void CreateRoom(string roomName, int maxPlayers, int mapNumber, int numberOfLaps, NetworkConnection conn, string nickname)
    {
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("CreateRoom called with a non-client connection.");
            return;
        }

        Guid newRoomId = Guid.NewGuid();
        GameRoom newRoom = new GameRoom
        {
            RoomId = newRoomId,
            RoomName = roomName,
            MaxPlayers = maxPlayers,
            MapNumber = mapNumber,
            NumberOfLaps = numberOfLaps,
            Players = new List<NetworkPlayer>()
        };

        openMatches.Add(newRoomId, newRoom);
        if (!matchConnections.ContainsKey(newRoomId))
        {
            matchConnections.Add(newRoomId, new HashSet<NetworkConnectionToClient>());
        }
        matchConnections[newRoomId].Add(clientConn);

        gameRoomManager.AddPlayerToRoom(newRoom, clientConn,  nickname, true);
    }

    public void SendRoomUpdate(GameRoom room)
    {
        RoomUpdateMessage updateMessage = new RoomUpdateMessage()
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            MaxPlayers = room.MaxPlayers,
            CurrentPlayers = room.Players.Count,
            MapNumber = room.MapNumber,
            NumberOfLaps = room.NumberOfLaps
        };
        NetworkServer.SendToAll(updateMessage);
    }

    // Invoke this method to update all clients with the latest list of game rooms
    public void UpdateLobbies()
    {
        var allRooms = openMatches.Values.ToList();
        LobbyListMessage response = new LobbyListMessage { Lobbies = allRooms };
        // Send the response to all connected clients, not just those waiting
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.isReady) // Make sure the connection is ready
                conn.Send(response);
        }
        OnLobbiesUpdated?.Invoke(allRooms);
        Debug.Log("Updated lobbies and notified all subscribers.");
    }

    public void OnServerConnect(NetworkConnection conn)
    {
        NetworkConnectionToClient clientConn = conn as NetworkConnectionToClient;
        base.OnServerConnect(clientConn);
        // Send the current lobby list to just the newly connected client
        var allRooms = openMatches.Values.ToList();
        LobbyListMessage response = new LobbyListMessage { Lobbies = allRooms };
        conn.Send(response);
    }

    #region Delete in future 
    public string GenerateRandomName(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; // All uppercase English letters
        var random = new System.Random();
        var randomName = new char[length];
        for (int i = 0; i < length; i++)
        {
            randomName[i] = chars[random.Next(chars.Length)];
        }
        return new string(randomName);
    }

    public void LogPlayerNicknames(GameRoom room)
    {
        // Check if the room has any players
        if (room.Players.Count == 0)
        {
            Debug.Log("No players in the room.");
            return;
        }

        Debug.Log("Listing all player nicknames in the room:");
        foreach (NetworkPlayer player in room.Players)
        {
            Debug.Log(player.playerName); // Assuming playerName is the attribute storing the nickname
        }
    }
    #endregion

    #region Server System Callbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
        gameRoomManager = new GameRoomManager(new List<GameRoom>(), this); // Ensure this is being called.
        NetworkServer.RegisterHandler<LeaveRoomRequest>(OnLeaveRoomRequestReceived);
        NetworkServer.RegisterHandler<RequestLobbyListMessage>(OnRequestLobbyListReceived);
        NetworkServer.RegisterHandler<CreateRoomRequest>(OnCreateRoomRequestReceived);
        NetworkServer.RegisterHandler<JoinRoomRequest>(OnJoinRoomRequestReceived);
    }

    private void OnRequestLobbyListReceived(NetworkConnection conn, RequestLobbyListMessage message)
    {
        UpdateLobbies();
    }

    private void OnCreateRoomRequestReceived(NetworkConnection conn, CreateRoomRequest request)
    {
        if (openMatches.Any(r => r.Value.RoomName == request.roomName))
        {
            conn.Send(new CreateRoomResponse { Success = false, Message = "Room already exists." });
        }
        else
        {
            CreateRoom(request.roomName, request.maxPlayers, request.mapNumber, request.numberOfLaps, conn, request.nickName);
            conn.Send(new CreateRoomResponse { Success = true, Message = "Room created successfully." });
        }
    }

    private void OnJoinRoomRequestReceived(NetworkConnection conn, JoinRoomRequest request)
    {
        // Ensure the connection is of the correct type.
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("JoinRoomRequest received from a non-client connection.");
            return;
        }

        // Find the room by room name.
        var room = openMatches.Values.FirstOrDefault(r => r.RoomName == request.roomName);
        if (!room.Equals(default(GameRoom)) && room.Players.Count < room.MaxPlayers)
        {
            // Call AddPlayerToRoom method from GameRoomManager with the correct parameters.
            gameRoomManager.AddPlayerToRoom(room, clientConn, request.playerName, false); // False because this player is not the owner.

            // Prepare and send the successful response.
            conn.Send(new JoinRoomResponse { success = true, message = "Successfully joined the room." });

            // Update the lobby list since the room's state has changed.
            UpdateLobbies();
        }
        else
        {
            // Send a response indicating failure to join the room.
            conn.Send(new JoinRoomResponse { success = false, message = "Failed to join room: Room is full or does not exist." });
        }
    }


    private void OnLeaveRoomRequestReceived(NetworkConnection conn, LeaveRoomRequest request)
    {
        var clientConn = conn as NetworkConnectionToClient;
        if (clientConn == null)
        {
            Debug.LogError("Received leave room request from a non-client connection.");
            return;
        }

        var room = openMatches.Values.FirstOrDefault(r => r.RoomName == request.roomName);
        LogPlayerNicknames(room);
        if (room.Equals(default(GameRoom))) // Ensure room exists
        {
            Debug.LogError("Room not found.");
            return;
        }

        // Find the player with the matching connection ID to remove.
        var playerToRemove = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());
        if (playerToRemove != null)
        {
            room.Players.Remove(playerToRemove); // Remove the player object from the list.

            openMatches[room.RoomId] = room; // Update the room in the dictionary if needed.
            UpdateLobbies();

            // Send a response including the player's nickname
            conn.Send(new LeaveRoomResponse
            {
                success = true,
                message = $"Successfully left the room. Player: {playerToRemove.playerName}",
                playerName = playerToRemove.playerName // Optionally send the name as a separate field
            });
            Debug.Log("Successfully processed leave room request and updated lobby.");
            if (playerToRemove.isOwner)
            {
                openMatches.Remove(room.RoomId);
                BroadcastLobbyLeaft updateMessage = new BroadcastLobbyLeaft()
                {
                };
                NetworkServer.SendToAll(updateMessage);
            }
        }
        else
        {
            LogPlayerNicknames(room);
            conn.Send(new LeaveRoomResponse
            {
                success = false,
                message = "Player not found in the room based on connection ID."
            });
            Debug.LogError("Failed to find player in room based on connection ID.");
        }
    }

    #endregion

    #region Client System Callbacks
    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<LeaveRoomResponse>(OnLeaveRoomResponseReceived);
        NetworkClient.RegisterHandler<LobbyListMessage>(OnLobbyListReceived);
        NetworkClient.RegisterHandler<CreateRoomResponse>(OnCreateRoomResponseReceived);
        NetworkClient.RegisterHandler<JoinRoomResponse>(OnJoinRoomResponseReceived);
        NetworkClient.RegisterHandler<BroadcastLobbyLeaft>(OnBroadcastLobbyLeaftReceived);
    }

    private void OnLobbyListReceived(LobbyListMessage message)
    {
        Debug.Log($"Received lobby list with {message.Lobbies.Count} lobbies.");
        UIManager.Instance.UpdateLobbyList(message.Lobbies);
    }

    private void OnCreateRoomResponseReceived(CreateRoomResponse response)
    {
        Debug.Log($"Create Room Response: {response.Message}");
        if (response.Success)
        {
            Debug.Log("Room created successfully.");
            UIManager.Instance.UpdateUIOnRoomJoin();
        }
    }

    private void OnJoinRoomResponseReceived(JoinRoomResponse response)
    {
        Debug.Log($"Join Room Response: {response.message}");
        if (response.success)
        {
            Debug.Log("Successfully joined the room.");
            SceneManager.LoadScene("LobbyScene");
        }
        UIManager.Instance.UpdateUIOnRoomJoin();
    }

    private void OnLeaveRoomResponseReceived(LeaveRoomResponse response)
    {
        Debug.Log(response.message);
        if (response.success)
        {
            UIManager.Instance.UpdateUIOnRoomLeave();
            UpdateLobbies();
        }
        else
        {
            Debug.LogError("Failed to leave room: " + response.message);
            // Handle error (maybe show a message to the user)
        }
    }

    private void OnBroadcastLobbyLeaftReceived(BroadcastLobbyLeaft response)
    {
        UIManager.Instance.UpdateUIOnRoomLeave();
        UpdateLobbies();
    }
    #endregion
}
