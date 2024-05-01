using Mirror;
using System.Collections.Generic;
using System;

[System.Serializable]
public struct GameRoom
{
    public List<string> PlayerNicknames;
    public Guid RoomId;
    public string RoomName;
    public int MaxPlayers;
    public int CurrentPlayers;
    public int MapNumber;
    public int NumberOfLaps;

    // Method to check if the room is full
    public bool IsFull()
    {
        return CurrentPlayers >= MaxPlayers;
    }
}