using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Linq;

public class CustomNetworkManager : NetworkManager
{

    public static event Action<List<GameRoom>> OnLobbiesUpdated;
    public static Dictionary<Guid, GameRoom> openMatches = new Dictionary<Guid, GameRoom>();
    public static Dictionary<Guid, HashSet<NetworkConnectionToClient>> matchConnections = new Dictionary<Guid, HashSet<NetworkConnectionToClient>>();
    public static List<NetworkConnectionToClient> waitingConnections = new List<NetworkConnectionToClient>();

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

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<RequestLobbyListMessage>(OnRequestLobbyListReceived);
        NetworkServer.RegisterHandler<CreateRoomRequest>(OnCreateRoomRequestReceived);
        NetworkServer.RegisterHandler<JoinRoomRequest>(OnJoinRoomRequestReceived);
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

    public void CreateRoom(string roomName, int maxPlayers, int mapNumber, int numberOfLaps, NetworkConnection conn, string nickname)
    {
        NetworkConnectionToClient clientConn = conn as NetworkConnectionToClient;
        if (clientConn == null)
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
            CurrentPlayers = 1,
            MapNumber = mapNumber,
            NumberOfLaps = numberOfLaps,
            PlayerNicknames = new List<string>()  // Initialize the list
        };
        newRoom.PlayerNicknames.Add(nickname);  // Add the creator's nickname to the list of players

        openMatches.Add(newRoomId, newRoom);
        if (!matchConnections.ContainsKey(newRoomId))
            matchConnections.Add(newRoomId, new HashSet<NetworkConnectionToClient>());

        matchConnections[newRoomId].Add(clientConn);
        UpdateLobbies();
    }





    private void OnJoinRoomRequestReceived(NetworkConnection conn, JoinRoomRequest request)
{
    var room = openMatches.Values.FirstOrDefault(r => r.RoomName == request.roomName);
    if (!room.Equals(default(GameRoom)) && room.CurrentPlayers < room.MaxPlayers)
    {
        // Correctly update the room in the dictionary
        var updatedRoom = room;
        updatedRoom.CurrentPlayers++;
        openMatches[room.RoomId] = updatedRoom;

        if (matchConnections.TryGetValue(room.RoomId, out var connections))
        {
            connections.Add(conn as NetworkConnectionToClient);
        }

        conn.Send(new JoinRoomResponse { success = true, message = "Successfully joined the room." });
        UpdateLobbies();
    }
    else
    {
        conn.Send(new JoinRoomResponse { success = false, message = "Failed to join room: Room is full or does not exist." });
    }
}



    private void OnCreateRoomResponseReceived(CreateRoomResponse response)
    {
        Debug.Log($"Create Room Response: {response.Message}");
        if (response.Success)
        {
            Debug.Log("Room created successfully.");
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
    }


    private void OnRequestLobbyListReceived(NetworkConnection conn, RequestLobbyListMessage message)
    {
        UpdateLobbies();
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<LobbyListMessage>(OnLobbyListReceived);
        NetworkClient.RegisterHandler<CreateRoomResponse>(OnCreateRoomResponseReceived);
        NetworkClient.RegisterHandler<JoinRoomResponse>(OnJoinRoomResponseReceived);
    }

    private void OnLobbyListReceived(LobbyListMessage message)
    {
        Debug.Log($"Received lobby list with {message.Lobbies.Count} lobbies.");
        UIManager.Instance.UpdateLobbyList(message.Lobbies);
    }

    public void SendRoomUpdate(GameRoom room)
    {
        RoomUpdateMessage updateMessage = new RoomUpdateMessage()
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            MaxPlayers = room.MaxPlayers,
            CurrentPlayers = room.CurrentPlayers,
            MapNumber = room.MapNumber,
            NumberOfLaps = room.NumberOfLaps
        };

        NetworkServer.SendToAll(updateMessage);
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
}
