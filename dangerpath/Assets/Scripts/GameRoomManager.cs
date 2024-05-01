using Mirror;
using System.Collections.Generic;
using UnityEngine;

public class GameRoomManager
{
    private List<GameRoom> gameRooms;
    private CustomNetworkManager networkManager; // Assuming this is how you access your network manager

    public GameRoomManager(List<GameRoom> gameRooms, NetworkManager networkManager)
    {
        this.gameRooms = gameRooms;
        this.networkManager = (CustomNetworkManager)networkManager;
    }

    public void NotifyClientsOfUpdatedRoom(GameRoom room)
    {
        // Example: Assuming you have a method in NetworkManager to send updates
        networkManager.SendRoomUpdate(room);
    }

    public void PlayerJoined(GameRoom room, NetworkConnection conn, string nickname)
    {
        NetworkConnectionToClient clientConn = conn as NetworkConnectionToClient;
        if (clientConn == null)
        {
            Debug.LogError("Connection is not a client connection.");
            return;
        }

        if (!room.IsFull())
        {
            if (!room.PlayerNicknames.Contains(nickname))
            {
                room.PlayerNicknames.Add(nickname);
                room.CurrentPlayers = room.PlayerNicknames.Count;
                NotifyClientsOfUpdatedRoom(room);
            }
            else
            {
                Debug.LogError("Nickname already exists in the room.");
                // Optionally handle the situation, e.g., request a different nickname
            }
        }
    }


}
