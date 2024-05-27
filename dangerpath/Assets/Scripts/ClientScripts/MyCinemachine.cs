using Cinemachine;
using Mirror;
using System.Collections;
using UnityEngine;

public class MyCinemachine : MonoBehaviour
{
    public static MyCinemachine Instance { get; private set; }
    private CinemachineVirtualCamera vcam;
    private CinemachineConfiner confiner;

    private NetworkPlayer myPlayer;

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

        vcam = GetComponent<CinemachineVirtualCamera>();
        confiner = GetComponent<CinemachineConfiner>();
    }

    private void Start()
    {
        SetupConfiner();
    }

    private void SetupConfiner()
    {
        // Find the map GameObject by tag
        GameObject map = GameObject.FindWithTag("Map");
        if (map != null)
        {
            // Find the PolygonCollider2D in the map GameObject
            PolygonCollider2D polyCollider = map.GetComponentInChildren<PolygonCollider2D>();

            if (polyCollider != null)
            {
                // Set the PolygonCollider2D as the Bounding Shape 2D
                confiner.m_BoundingShape2D = polyCollider;
                Debug.Log("BoundingShape2D added");
            }
            else
            {
                Debug.LogWarning("No PolygonCollider2D found in the map GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Map GameObject is not assigned.");
        }
    }

    public void SetCamera()
    {
        StartCoroutine(SetCameraCoroutine());
    }

    public IEnumerator FindNetworkPlayerCoroutine()
    {
        string trimmedTargetName = CrossScaneInfoHolder.GamerNickName.Trim();

        while (myPlayer == null)
        {
            var players = FindObjectsOfType<NetworkPlayer>();

            foreach (var player in players)
            {
                if (player.playerName.Trim() == trimmedTargetName)
                {
                    myPlayer = player;
                    break;
                }
            }
            
            if (myPlayer == null)
            {
                Debug.Log($"Looking for player with name: {trimmedTargetName}");
                yield return new WaitForSeconds(0.5f); // Wait for a short duration before trying again
            }
        }
    }
        private IEnumerator SetCameraCoroutine()
    {
        // Wait a frame to ensure all objects are initialized
        yield return null;
        StartCoroutine(FindNetworkPlayerCoroutine());
        if (myPlayer != null)
        {
            if (vcam != null)
            {
                // Set the Follow target to the player
                vcam.Follow = myPlayer.transform;

                Debug.Log("Camera setup complete");
            }
            else
            {
                Debug.LogWarning("No CinemachineVirtualCamera found on the GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("No NetworkPlayer found in the scene.");
        }
    }
}
