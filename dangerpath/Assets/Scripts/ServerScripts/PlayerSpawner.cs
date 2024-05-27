using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    [Header("Spawn Positions")]
    public List<Vector3> spawnPositions; // Список позицій для спавнуктів

    private int currentSpawnIndex = 0; // Індекс для поточної позиції спавну

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

    public void SpawnPlayers(GameRoom gameRoom)
    {
        // Get the list of registered prefabs from NetworkManager
        var playerPrefabs = NetworkManager.singleton.spawnPrefabs;

        if (playerPrefabs == null || playerPrefabs.Count == 0)
        {
            Debug.LogError("Player prefabs are not set or empty.");
            return;
        }

        foreach (var playerInfo in gameRoom.Players)
        {
            // Find the prefab by carId
            GameObject playerPrefab = playerPrefabs.FirstOrDefault(prefab => prefab.name == playerInfo.carId.ToString());

            if (playerPrefab == null)
            {
                Debug.LogError($"No prefab found for carId {playerInfo.carId}");
                continue;
            }

            // Determine the spawn position and rotation
            Vector3 spawnPosition = GetSpawnPosition();
            Quaternion spawnRotation = Quaternion.Euler(0, 0, 0);

            // Spawn the player and assign client authority
            GameObject playerObject = Instantiate(playerPrefab, spawnPosition, spawnRotation);
            Debug.Log($"Spawning player {playerInfo.playerName} at position {spawnPosition} with rotation {spawnRotation.eulerAngles}");
            NetworkConnectionToClient conn = NetworkServer.connections.FirstOrDefault(c => c.Value.connectionId.ToString() == playerInfo.connectionId).Value;
            if (conn != null)
            {
                NetworkServer.Spawn(playerObject, conn);

                // Set player information to the instantiated prefab
                var networkPlayer = playerObject.GetComponent<NetworkPlayer>();
                if (networkPlayer != null)
                {
                    networkPlayer.SetPlayer(playerInfo.playerName, playerInfo.isOwner, playerInfo.connectionId);
                    networkPlayer.carId = playerInfo.carId;
                }
                else
                {
                    Debug.LogError("NetworkPlayer component not found on the instantiated prefab.");
                    Destroy(playerObject);
                }
            }
            else
            {
                Debug.LogError($"Connection for player with ID {playerInfo.connectionId} not found.");
                Destroy(playerObject);
            }
        }
    }

    private Vector3 GetSpawnPosition()
    {
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            Debug.LogError("Spawn positions are not set.");
            return Vector3.zero;
        }

        // Вибираємо позицію для спавну по черзі
        Vector3 spawnPosition = spawnPositions[currentSpawnIndex];
        currentSpawnIndex = (currentSpawnIndex + 1) % spawnPositions.Count;

        return spawnPosition;
    }
}
