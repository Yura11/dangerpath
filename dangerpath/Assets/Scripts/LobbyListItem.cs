using UnityEngine;
using UnityEngine.UI;

public class LobbyListItem : MonoBehaviour
{
    public Text roomNameText;
    public Text maxPlayersText;
    public Button joinButton;

    public void Setup(string roomName, int maxPlayers, System.Action onJoinClicked)
    {
        roomNameText.text = roomName;
        maxPlayersText.text = "Max Players: " + maxPlayers;
        joinButton.onClick.AddListener(() => onJoinClicked());
    }

    private void OnDestroy()
    {
        joinButton.onClick.RemoveAllListeners();
    }
}
