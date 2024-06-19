using UnityEngine;
using Mirror;
using System;

public class AutoConnect : MonoBehaviour 
{
    [SerializeField] NetworkManager networkManager;
    private void Start()
    {
        networkManager.StartClient();
    }
}
