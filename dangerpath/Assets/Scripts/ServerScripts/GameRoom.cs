using Mirror;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct GameRoom
{
    public List<NetworkPlayer> Players;
    public Guid RoomId;
    public string RoomName;
    public int MaxPlayers;
    public int MapNumber;
    public int NumberOfLaps;
    public bool GameState;

    // Method to check if the room is full
    public bool IsFull()
    {
        return Players.Count >= MaxPlayers;
    }
}