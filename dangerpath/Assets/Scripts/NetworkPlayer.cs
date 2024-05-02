using Mirror;
using UnityEngine;

[System.Serializable]
public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName;
    [SyncVar]
    public bool isOwner = false;
    [SyncVar]
    public string connectionId;

    public void SetPlayer(string name, bool ownerStatus, string connId)
    {
        playerName = name;
        isOwner = ownerStatus;
        connectionId = connId;  // Set the connection ID
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log($"{playerName} joined the game, owner status: {isOwner}");
    }
}
