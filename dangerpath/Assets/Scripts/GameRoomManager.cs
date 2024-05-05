using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameRoomManager : MonoBehaviour
{
    private CustomNetworkManager networkManager;
    private List<GameRoom> gameRooms;

    public GameRoomManager(List<GameRoom> gameRooms, CustomNetworkManager networkManager)
    {
        this.gameRooms = gameRooms;
        this.networkManager = networkManager;
    }

    public void AddPlayerToRoom(GameRoom room, NetworkConnection conn, string nickname, bool isOwner)
    {
        // Attempt to cast to NetworkConnectionToClient
        NetworkConnectionToClient clientConn = conn as NetworkConnectionToClient;
        if (clientConn == null)
        {
            Debug.LogError("Failed to cast NetworkConnection to NetworkConnectionToClient.");
            return;
        }

        if (room.IsFull())
        {
            Debug.LogError("Room is full.");
            return;
        }

        int connectionId = clientConn.connectionId;
        if (room.Players.Any(p => p.connectionId == connectionId.ToString()))
        {
            Debug.LogError("Player with this connection ID already exists in the room.");
            return;
        }

        // Assume playerPrefab is a configured GameObject in your scene or assets with a NetworkPlayer component
        GameObject playerObject = Instantiate(networkManager.playerPrefab);
        NetworkPlayer player = playerObject.GetComponent<NetworkPlayer>();
        if (player != null)
        {
            player.SetPlayer(nickname, isOwner, connectionId.ToString());
            NetworkServer.AddPlayerForConnection(clientConn, playerObject);
            room.Players.Add(player);
        }
        else
        {
            Debug.LogError("NetworkPlayer component not found on the prefab!");
            Destroy(playerObject); // Clean up to avoid ghost GameObjects
        }
    }

    public void NotifyRoomClientsOfUpdatedPlayerList(Guid roomId)
    {
        Debug.Log("NotifyRoomClientsOfUpdatedPlayerList");

        // Check if such a match exists in the connection dictionary
        if (CustomNetworkManager.matchConnections.TryGetValue(roomId, out var connections))
        {
            var room = CustomNetworkManager.openMatches[roomId];

            // Convert the NetworkPlayer list to a PlayerData list
            var players = room.Players.Select(p => new PlayerListUpdateMessage.PlayerData
            {
                PlayerName = p.playerName,
                ReadyStatus = p.playerReadyStatus,
                OwnerStatus = p.isOwner,
            }).ToList();

            // Prepare a message with an updated list of players
            var message = new PlayerListUpdateMessage { Players = players };

            // Send messages only to players listed in this GameRoom
            foreach (var player in room.Players)
            {
                if (NetworkServer.connections.TryGetValue(int.Parse(player.connectionId), out var conn))
                {
                    conn.Send(message);
                }
            }
        }
    }

}
