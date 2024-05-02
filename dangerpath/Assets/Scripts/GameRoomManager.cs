using Mirror;
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
}
