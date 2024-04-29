using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System;

public class CustomNetworkManager : NetworkManager
{
    public static event Action<List<GameRoom>> OnLobbiesUpdated;
    private List<GameRoom> gameRooms = new List<GameRoom>();

    // Invoke this method to update all subscribers with the latest list of game rooms
    public void UpdateLobbies()
    {
        OnLobbiesUpdated?.Invoke(gameRooms);
    }

    public void RequestLobbyListUpdate()
    {
        if (NetworkServer.active)  // Check if this instance is the server
        {
            SendLobbyUpdateToClients();
        }
    }

    public override void OnStartServer()
    {
        base.OnStartServer();
        NetworkServer.RegisterHandler<JoinRoomRequest>(OnJoinRoomRequestReceived);
        NetworkServer.RegisterHandler<CreateRoomRequest>(OnCreateRoomRequestReceived);
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
            CreateRoom(request.roomName, request.maxPlayers);
            response.Success = true;
            response.Message = "Room created successfully.";
        }

        conn.Send(response);
    }

    public void CreateRoom(string roomName, int maxPlayers)
    {
        GameRoom newRoom = new GameRoom(roomName, maxPlayers);
        gameRooms.Add(newRoom);
        // Further logic to initialize the room or inform players
    }

    private void SendLobbyUpdateToClients()
    {
        // You can optimize by sending only to clients that need it or broadcasting to all
        UIManager.Instance.UpdateLobbyList(gameRooms);
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<CreateRoomResponse>(OnCreateRoomResponseReceived);
        NetworkClient.RegisterHandler<JoinRoomResponse>(OnJoinRoomResponseReceived);
    }

    private void OnCreateRoomResponseReceived(CreateRoomResponse response)
    {
        Debug.Log($"Response received: {response.Message}");
    }

    private void OnJoinRoomResponseReceived(JoinRoomResponse response)
    {
        if (response.success)
        {
            Debug.Log("Successfully joined the room.");
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

    // Handler for join requests
    private void OnJoinRoomRequestReceived(NetworkConnection conn, JoinRoomRequest request)
    {
        JoinLobby(request.roomName, conn);
        Debug.Log("Join Room Request received");
    }

    public void BroadcastLobbyUpdate()
    {
        var lobbyListMessage = new LobbyListMessage { Lobbies = gameRooms };
        NetworkServer.SendToAll(lobbyListMessage);
    }
}
