using Mirror;
using System.Collections.Generic;

public class GameRoom
{
    public string RoomName { get; private set; }
    public int MaxPlayers { get; private set; }
    public int CurrentPlayers { get; set; }
    public List<NetworkConnection> Players { get; private set; }
    public int MapNumber { get; private set; }
    public int NumberOfLaps { get; private set; }

    // Default constructor
    public GameRoom()
    {
        RoomName = "Default constructor Room";
        MaxPlayers = 4;
        Players = new List<NetworkConnection>();
    }

    // Additional constructor for initialization
    public GameRoom(string name, int maxPlayers, int mapNumber, int numberOfLaps)
    {
        RoomName = name;
        MaxPlayers = maxPlayers;
        Players = new List<NetworkConnection>();
        MapNumber = mapNumber;
        NumberOfLaps = numberOfLaps;
    }

    public bool IsFull() => Players.Count >= MaxPlayers;
}
