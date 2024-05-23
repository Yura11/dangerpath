using Mirror;
using UnityEngine;
using System.Collections.Generic;

public class PlayerSpawner : MonoBehaviour
{
    public static PlayerSpawner Instance { get; private set; }

    public List<GameObject> playerPrefabs; // Array of player prefabs, index corresponds to carId

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

    public void SpawnPlayers(List<NetworkPlayer> PlayerList)
    {
        if (playerPrefabs == null || playerPrefabs.Count == 0)
        {
            Debug.LogError("Player prefabs are not set or empty.");
            return;
        }

        foreach (NetworkPlayer playerInfo in PlayerList)
        {
            if (playerInfo.carId < 0 || playerInfo.carId >= playerPrefabs.Count)
            {
                Debug.LogError($"Invalid carId {playerInfo.carId} for player {playerInfo.playerName}");
                continue;
            }

            // ќбираЇмо префаб в≥дпов≥дно до carId
            GameObject playerPrefab = playerPrefabs[playerInfo.carId];

            if (playerPrefab == null)
            {
                Debug.LogError($"No prefab found for carId {playerInfo.carId}");
                continue;
            }

            // —павнимо гравц€
            GameObject playerObject = Instantiate(playerPrefab);
            NetworkServer.Spawn(playerObject);

            // ЌалаштовуЇмо дан≥ гравц€
            NetworkPlayer networkPlayer = playerObject.GetComponent<NetworkPlayer>();
            if (networkPlayer != null)
            {
                networkPlayer.SetPlayer(playerInfo.playerName, playerInfo.isOwner, playerInfo.connectionId);
                networkPlayer.carId = playerInfo.carId;

                // ƒодаЇмо гравц€ до списку
                PlayerList.Add(networkPlayer);
            }
            else
            {
                Debug.LogError($"NetworkPlayer component not found on prefab for carId {playerInfo.carId}");
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
