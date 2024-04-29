using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

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
}
public struct LobbyListMessage : NetworkMessage
{
    public List<GameRoom> Lobbies;
}

public struct JoinRoomRequest : NetworkMessage
{
    public string roomName;
}

public struct JoinRoomResponse : NetworkMessage
{
    public bool success;
    public string message;
}
