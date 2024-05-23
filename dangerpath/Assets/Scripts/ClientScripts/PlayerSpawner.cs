using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    public List<GameObject> carPrefabs; // Array of player prefabs, index corresponds to carId

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SpawnPlayers()
    {
        foreach (var playerInfo in CrossScaneInfoHolder.PlayerList)
        {
            if (playerInfo.carId < carPrefabs.Count)
            {
                GameObject playerPrefab = carPrefabs[playerInfo.carId];
                GameObject playerObject = Instantiate(playerPrefab, GetSpawnPosition(), Quaternion.identity);
                NetworkServer.Spawn(playerObject);

                // Configure player parameters
                NetworkPlayer networkPlayer = playerObject.GetComponent<NetworkPlayer>();
                if (networkPlayer != null)
                {
                    networkPlayer.SetPlayer(playerInfo.playerName, playerInfo.isOwner, playerInfo.connectionId);
                    networkPlayer.playerReadyStatus = playerInfo.playerReadyStatus;
                }
            }
            else
            {
                Debug.LogError($"Invalid carId {playerInfo.carId} for player {playerInfo.playerName}");
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        // Define logic to determine player spawn position
        // For example, return a random position or a position from a predefined list
        return Vector3.zero; // Return zero position as an example
    }
}
