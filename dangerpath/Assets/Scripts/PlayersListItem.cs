using UnityEngine;
using UnityEngine.UI;

public class PlayersListItem : MonoBehaviour
{
    public Text playerNameText;
    public Text playerReadyStatusText;

    public void SetupPlayersListItem(string playerName, bool playerReadyStatus, bool isOwner) 
    {
        playerReadyStatusText.text = isOwner ? "Lobby Owner" : playerReadyStatus ? "Ready" : "Not Ready";
        playerNameText.text = playerName;
    }
}
