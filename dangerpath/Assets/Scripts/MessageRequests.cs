using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class MessageRequests : NetworkMessage
{
    // You can add more fields here if needed, for example:
    // public string lobbyName;
    // public int maxPlayers;
}


public struct CreateRoomResponse : NetworkMessage
{
    public bool Success;
    public string Message;
}

public struct CreateRoomRequest : NetworkMessage
{
    public string roomName;
    public int maxPlayers;
    public int mapNumber;
    public int numberOfLaps;
    public string nickName;
}
public struct LobbyListMessage : NetworkMessage
{
    public List<GameRoom> Lobbies;
}

public struct JoinRoomRequest : NetworkMessage
{
    public string roomName;
    public string playerName;
}

public struct JoinRoomResponse : NetworkMessage
{
    public bool success;
    public string message;
}

public struct RoomUpdateMessage : NetworkMessage
{
    public Guid RoomId;
    public string RoomName;
    public int MaxPlayers;
    public int CurrentPlayers;
    public int MapNumber;
    public int NumberOfLaps;
}


public struct RequestLobbyListMessage : NetworkMessage { }

public struct LeaveRoomRequest : NetworkMessage
{
    public string roomName; // Existing
    public string playerName; // Add this if it's missing
}

public struct LeaveRoomResponse : NetworkMessage
{
    public bool success;
    public string message;
    public string playerName;  // Add this line to include the player's nickname in the response
}

public struct BroadcastLobbyLeaft : NetworkMessage { }

public struct PlayerListUpdateMessage : NetworkMessage
{
    public List<PlayerData> Players;

    public struct PlayerData
    {
        public string PlayerName;
        public bool ReadyStatus;
        public bool OwnerStatus;
    }
}

public struct PlayerStatusRequest : NetworkMessage { }

public struct PlayerStatusMessage : NetworkMessage
{
    public bool OwnerStatus;
}

public struct SetPlayerReadyStatusRequest : NetworkMessage
{
    public bool PlayerReadyStatus;
}