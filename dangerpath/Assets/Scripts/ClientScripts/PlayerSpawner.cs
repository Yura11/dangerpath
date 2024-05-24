using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    [Header("Spawn Positions")]
    public List<Vector3> spawnPositions; // Список позицій для спавну

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

        // Ініціалізація списку позицій спавну, якщо він не встановлений в інспекторі
        if (spawnPositions == null || spawnPositions.Count == 0)
        {
            spawnPositions = new List<Vector3>
            {
                new Vector3(-10, 0, -10),
                new Vector3(10, 0, -10),
                new Vector3(-10, 0, 10),
                new Vector3(10, 0, 10)
            };
        }
    }

    public void SpawnPlayers(List<NetworkPlayer> PlayerList)
    {
        // Отримуємо список зареєстрованих префабів із NetworkManager
        var playerPrefabs = NetworkManager.singleton.spawnPrefabs;

        if (playerPrefabs == null || playerPrefabs.Count == 0)
        {
            Debug.LogError("Player prefabs are not set or empty.");
            return;
        }

        List<NetworkPlayer> newPlayers = new List<NetworkPlayer>();

        foreach (NetworkPlayer playerInfo in PlayerList)
        {
            // Знайдіть префаб за carId
            GameObject playerPrefab = playerPrefabs.FirstOrDefault(prefab => prefab.name == playerInfo.carId.ToString());

            if (playerPrefab == null)
            {
                Debug.LogError($"No prefab found for carId {playerInfo.carId}");
                continue;
            }

            // Спавнимо гравця і передаємо авторитет клієнту
            GameObject playerObject = Instantiate(playerPrefab, GetSpawnPosition(), Quaternion.identity);
            Debug.Log($"Spawning player {playerInfo.playerName} at position {playerObject.transform.position}");
            NetworkConnection conn = NetworkServer.connections.FirstOrDefault(c => c.Value.connectionId.ToString() == playerInfo.connectionId).Value;
            if (conn != null)
            {
                NetworkServer.Spawn(playerObject, conn);

                // Налаштовуємо дані гравця
                NetworkPlayer networkPlayer = playerObject.GetComponent<NetworkPlayer>();
                if (networkPlayer != null)
                {
                    networkPlayer.SetPlayer(playerInfo.playerName, playerInfo.isOwner, playerInfo.connectionId);
                    networkPlayer.carId = playerInfo.carId;

                    // Додаємо гравця до тимчасового списку
                    newPlayers.Add(networkPlayer);
                }
                else
                {
                    Debug.LogError($"NetworkPlayer component not found on prefab for carId {playerInfo.carId}");
                    Destroy(playerObject);
                }
            }
            else
            {
                Debug.LogError($"Connection for player with ID {playerInfo.connectionId} not found.");
                Destroy(playerObject);
            }
        }

        // Додаємо нових гравців до основного списку після завершення ітерації
        PlayerList.AddRange(newPlayers);
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
