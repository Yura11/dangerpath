using Mirror;
using System.Collections.Generic;

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

    public void PlayerJoined(GameRoom room, NetworkConnection conn)
    {
        if (!room.IsFull())
        {
            room.Players.Add(conn);
            room.CurrentPlayers = room.Players.Count;
            NotifyClientsOfUpdatedRoom(room);
        }
    }
}
