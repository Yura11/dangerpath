using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Cinemachine;
using Photon.Pun.Demo.PunBasics;

public class SpawnPlayers : MonoBehaviour
{
    
    public GameObject playerPrefab;

    // List of spawn positions
    public List<Vector3> spawnPositions = new List<Vector3>();

    // Rotation properties
    public float rotationX;
    public float rotationY;
    public float rotationZ;

    // Scale properties
    public float scaleX;
    public float scaleY;
    public float scaleZ;

    private bool isRoomOwner = false;

   /* private PhotonView photonView; // ������� �� ����



    [PunRPC]
    private void SetPlayerName(string playerName)
    {
        PhotonNetwork.LocalPlayer.NickName = playerName;
    }

    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true; // ��� false, � ��������� �� ������ �������
        photonView = GetComponent<PhotonView>();
        if (!PhotonNetwork.IsConnected)
            return;
       // SpawnPlayer();
    }

    [PunRPC]
    public void SpawnPlayer()
    {
        if (!PhotonNetwork.IsConnected)
            return;
        if (playerPrefab != null)
        {
            int playerId = PhotonNetwork.LocalPlayer.ActorNumber;
            Vector3 spawnPosition = spawnPositions[(playerId - 1) % spawnPositions.Count];

            string playerName = "Player" + playerId.ToString();


            GameObject spawnedPlayer = PhotonNetwork.Instantiate(playerPrefab.name, spawnPosition, Quaternion.identity);
            spawnedPlayer.name = playerName;

            Vector3 fixedRotation = new Vector3(rotationX, rotationY, rotationZ);
            Vector3 fixedScale = new Vector3(scaleX, scaleY, scaleZ);

            spawnedPlayer.transform.rotation = Quaternion.Euler(fixedRotation);
            spawnedPlayer.transform.localScale = fixedScale;

            if (spawnedPlayer.GetComponent<PhotonView>().IsMine)
            {
                CinemachineVirtualCamera vcam = FindObjectOfType<CinemachineVirtualCamera>();
                vcam.Follow = spawnedPlayer.transform;
                CheckRoomOwner();
                foreach (Player player in PhotonNetwork.PlayerList)
                {
                    Debug.Log("Player ID: " + player.ActorNumber + ", NickName: " + player.NickName);
                }
            }
 
            SetPlayerColor(spawnedPlayer, (byte)playerId);
        }
        else
        {
            Debug.LogError("myObject is null!");
        }
    }

    private void CheckRoomOwner()
    {
        isRoomOwner = PhotonNetwork.IsMasterClient;

        if (isRoomOwner)
        {
            Debug.Log("You are the room owner.");
        }
        else
        {
            Debug.Log("You are not the room owner.");
        }
    }



    private void SetPlayerColor(GameObject localPlayer, byte playerID)
    {
        // ���������, �� ������� ���������
        if (localPlayer != null)
        {
            // ������ ���� ������ ��������
            Renderer playerRenderer = localPlayer.GetComponent<Renderer>();
            if (playerRenderer != null)
            {
                //Random.InitState(playerID);

                // ��������� ����������� �������
                Color randomColor = new Color(Random.value, Random.value, Random.value);

                // ���� ����� ������
                playerRenderer.material.color = randomColor;
            }
            else
            {
                Debug.LogError("Player renderer not found!");
            }
        }
        else
        {
            Debug.LogError("Local player not found!");
        }
    }

    public void RecolorPlayersObjects()
    {
        // ������ �� ��'���� � �������� TopDownCarController
        TopDownCarController[] carControllers = FindObjectsOfType<TopDownCarController>();

        foreach (TopDownCarController carController in carControllers)
        {
            // �������� PhotonView ��� ������� ��'����
            PhotonView photonView = carController.GetComponent<PhotonView>();

            // ���������, �� PhotonView ����
            if (photonView != null)
            {
                Renderer carcolor = carController.gameObject.GetComponent<Renderer>();
                if (carcolor != null)
                {
                    byte seed = (byte)(photonView.ViewID.ToString().Length > 3
                    ? byte.Parse(photonView.ViewID.ToString().Substring(0, photonView.ViewID.ToString().Length - 3))
                    : 0);
                    //Random.InitState(playerID);
                    Random.InitState(seed);
                    // ��������� ����������� �������
                    Color randomColor = new Color(Random.value, Random.value, Random.value);

                    // ���� ����� ������
                    carcolor.material.color = randomColor;
                }
                else
                {
                    Debug.LogError("Player renderer not found!");
                }
            }
            else
            {
                Debug.LogError("PhotonView not found on object: " + carController.gameObject.name);
            }
        }
    }

    public void RenamePlayersObjects()
    {
        TopDownCarController[] carControllers = FindObjectsOfType<TopDownCarController>();

        foreach (TopDownCarController carController in carControllers)
        {
            PhotonView photonView = carController.GetComponent<PhotonView>();

            // �������� PhotonView ��� ������� ��'����
            if (photonView != null)
            {
                // ������������� ��'��� ������� ���� ViewID
                carController.gameObject.name = "Player " + (photonView.ViewID.ToString().Length > 3 ? photonView.ViewID.ToString().Substring(0, photonView.ViewID.ToString().Length - 3) : "0");
            }
            else
            {
                Debug.LogError("PhotonView not found on object: " + carController.gameObject.name);
            }
        }
    }*/
}
