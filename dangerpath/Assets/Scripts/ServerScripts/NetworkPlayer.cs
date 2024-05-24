using Mirror;
using UnityEngine;

[System.Serializable]
public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;
    [SyncVar]
    public bool isOwner;
    [SyncVar]
    public string connectionId;
    [SyncVar]
    public bool playerReadyStatus;
    [SyncVar]
    public int carId;

    private TopDownCarController carController;

    public void SetPlayer(string name, bool ownerStatus, string connId)
    {
        playerName = name;
        isOwner = ownerStatus;
        connectionId = connId;
        playerReadyStatus = isOwner;
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();
        carController = GetComponent<TopDownCarController>();
        if (carController != null)
        {
            carController.enabled = true;
            Debug.Log($"{playerName} has authority and control enabled.");
        }
        else
        {
            Debug.LogError("TopDownCarController component not found on the player object.");
        }
    }

    public override void OnStartLocalPlayer()
    {
        base.OnStartLocalPlayer();
        Debug.Log($"{playerName} is the local player");
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log($"{playerName} joined the game, owner status: {isOwner}");
    }

    private void Update()
    {


        if (isLocalPlayer)
        {
            Debug.Log("isLocalPlayer");
        }
        else
        {
            Debug.Log("notLocalPlayer");
        }
    }  
}