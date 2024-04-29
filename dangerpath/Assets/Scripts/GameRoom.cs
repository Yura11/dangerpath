using Mirror;
using System.Collections.Generic;

public class GameRoom
{
    public string RoomName { get; set; }
    public int MaxPlayers { get; set; }
    public List<NetworkConnection> Players { get; set; }

    // Default constructor
    public GameRoom()
    {
        RoomName = "Default Room";
        MaxPlayers = 4;
        Players = new List<NetworkConnection>();
    }

    // Additional constructor for initialization
    public GameRoom(string name, int maxPlayers)
    {
        RoomName = name;
        MaxPlayers = maxPlayers;
        Players = new List<NetworkConnection>();
    }

    public bool IsFull() => Players.Count >= MaxPlayers;
}
