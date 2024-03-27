using UnityEngine;
using Mirror;
using System;

public class AutoConnect : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    public void JoinLocal()
    {
        networkManager.networkAddress = "localhost";
        // ������� ���������� �볺��� �� ������� �� URI-�������
        networkManager.StartClient();
    }
    private void Start()
    {
        networkManager.networkAddress = "localhost";
        // ������� ���������� �볺��� �� ������� �� URI-�������
        networkManager.StartClient();
    }
}
