using Mirror;
using UnityEngine;

public class NetworkPlayer : NetworkBehaviour
{
    [SyncVar]
    public string playerName = "Player";

    public override void OnStartClient()
    {
        base.OnStartClient();
        Debug.Log($"{playerName} joined the game");
    }
}
