using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;

public class CustomNetworkManager : NetworkManager
{
    public static event Action<List<GameRoom>> OnLobbiesUpdated;
    private List<GameRoom> gameRooms = new List<GameRoom>();

    // Invoke this method to update all subscribers with the latest list of game rooms
    public void UpdateLobbies()
    {
        gameRooms = FetchUpdatedRooms();  // Assume FetchUpdatedRooms returns the latest list
        OnLobbiesUpdated?.Invoke(gameRooms);
        Debug.Log($"Updated lobbies: {gameRooms.Count} rooms now available.");
    }

    private List<GameRoom> FetchUpdatedRooms()
    {
        // For now, return the existing list of game rooms.
        // You can add logic here to fetch or update this list from a database or other data source as needed.
        return gameRooms;
    }

    public void RequestLobbyListUpdate()
    {
        if (NetworkClient.isConnected)
        {
            Debug.Log("Client is connected. Sending request for lobby updates.");
            SendLobbyUpdateToClients();
        }
        else
        {
            Debug.Log("Client is not connected to the server.");
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<JoinRoomRequest>(OnJoinRoomRequestReceived);
        NetworkServer.RegisterHandler<CreateRoomRequest>(OnCreateRoomRequestReceived);
        NetworkServer.RegisterHandler<RequestLobbyListMessage>(OnRequestLobbyListReceived);
    }

    private void OnCreateRoomRequestReceived(NetworkConnection conn, CreateRoomRequest request)
    {
        CreateRoomResponse response = new CreateRoomResponse();

        if (gameRooms.Exists(room => room.RoomName == request.roomName))
        {
            response.Success = false;
            response.Message = "Room already exists.";
        }
        else
        {
            // Use the provided details to create a new GameRoom
            CreateRoom(request.roomName, request.maxPlayers, request.mapNumber, request.numberOfLaps, conn);
            response.Success = true;
            response.Message = "Room created successfully.";
        }

        conn.Send(response);
    }

    public void CreateRoom(string roomName, int maxPlayers, int mapNumber, int numberOfLaps, NetworkConnection conn)
    {
        if (!gameRooms.Exists(room => room.RoomName == roomName))
        {
            GameRoom newRoom = new GameRoom(roomName, maxPlayers, mapNumber, numberOfLaps);
            gameRooms.Add(newRoom);
            PlayerJoined(newRoom, conn); // Add the creator to the room immediately

            // Notify the creator that the room was created and they have joined it
            JoinRoomResponse joinResponse = new JoinRoomResponse
            {
                success = true,
                message = "Room created and joined successfully."
            };
            conn.Send(joinResponse);
        }
        else
        {
            // Room already exists, send an error message
            CreateRoomResponse response = new CreateRoomResponse
            {
                Success = false,
                Message = "Room already exists."
            };
            conn.Send(response);
        }
    }

    private void SendLobbyUpdateToClients()
    {
        LobbyListMessage message = new LobbyListMessage { Lobbies = gameRooms };
        Debug.Log($"Sending lobby update to all clients with {message.Lobbies.Count} entries.");
        NetworkServer.SendToAll(message);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<CreateRoomResponse>(OnCreateRoomResponseReceived);
        NetworkClient.RegisterHandler<JoinRoomResponse>(OnJoinRoomResponseReceived);
        NetworkClient.RegisterHandler<LobbyListMessage>(OnLobbyListReceived);
    }

    private void OnRequestLobbyListReceived(NetworkConnection conn, RequestLobbyListMessage message)
    {
        UpdateLobbies();
        LobbyListMessage response = new LobbyListMessage { Lobbies = gameRooms };
        conn.Send(response);
        Debug.Log("Sent lobby list update to client.");
    }

    private void OnCreateRoomResponseReceived(CreateRoomResponse response)
    {
        Debug.Log($"Response received: {response.Message}");
    }

    private void OnJoinRoomResponseReceived(JoinRoomResponse response)
    {
        if (response.success)
        {
            Debug.Log("Successfully created and joined the room.");
            // Optionally switch to the lobby scene or update UI
            SceneManager.LoadScene("LobbyScene");
        }
        else
        {
            Debug.Log($"Failed to join the room: {response.message}");
        }
    }

    public void JoinLobby(string roomName, NetworkConnection conn)
    {
        GameRoom room = gameRooms.Find(r => r.RoomName == roomName);
        if (room != null && !room.IsFull())
        {
            // Logic to add the player to the room
            room.Players.Add(conn);
            Debug.Log($"Player joined room: {roomName}");

            // Send success response
            JoinRoomResponse response = new JoinRoomResponse { success = true, message = "Joined room successfully." };
            conn.Send(response);
        }
        else
        {
            // Send failure response
            JoinRoomResponse response = new JoinRoomResponse { success = false, message = "Failed to join room: Room is full or does not exist." };
            conn.Send(response);
            Debug.Log($"Failed to join room: {roomName}");
        }
    }

    public void PlayerJoined(GameRoom room, NetworkConnection player)
    {
        if (!room.IsFull())
        {
            room.Players.Add(player);
            room.CurrentPlayers = room.Players.Count; // Update player count
            UpdateLobbies();  // Notify UI to refresh
        }
    }

    // Handler for join requests
    private void OnJoinRoomRequestReceived(NetworkConnection conn, JoinRoomRequest request)
    {
        JoinLobby(request.roomName, conn);
        Debug.Log("Join Room Request received");
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
            RoomName = room.RoomName,
            MaxPlayers = room.MaxPlayers,
            CurrentPlayers = room.CurrentPlayers
        };

        NetworkServer.SendToAll(updateMessage);
    }
}
