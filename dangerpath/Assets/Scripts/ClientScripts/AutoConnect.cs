using UnityEngine;
using Mirror;
using System;

public class AutoConnect : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;

    public void JoinLocal()
    {
        networkManager.networkAddress = "localhost";
        // Початок підключення клієнта до сервера за URI-адресою
        networkManager.StartClient();
    }
    private void Start()
    {
        networkManager.networkAddress = "localhost";
        // Початок підключення клієнта до сервера за URI-адресою
        networkManager.StartClient();
    }
}
