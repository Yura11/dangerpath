using UnityEngine;
using UnityEngine.UI;

public class LobbyListItem : MonoBehaviour
{
    public Text roomNameText;
    public Text playersInLobbyText;
    public Text mapName;
    public Text gameStateText;
    public Button joinButton;

    private float lastClickTime = 0f;
    private const float doubleClickTime = 0.3f; // Time in seconds
    private int clickCount = 0;

    public void SetupLobbyListItem(string roomName, int maxPlayers, int currentPlayers, int mapNumber, bool gameState, System.Action onJoinClicked)
    {
        mapName.text = ""; // assuming you'll set this elsewhere or it's static
        roomNameText.text = roomName;
        playersInLobbyText.text = $"{currentPlayers}/{maxPlayers}"; // Updated to show current players
        gameStateText.text = gameState ? "In Game" : "In Lobby";
        if (mapNumber == 0)
        {
            mapName.text = "Pechersk";
        }
        else
        {
            mapName.text = "Not Pechersk";
        }
        joinButton.onClick.AddListener(() => HandleClick(onJoinClicked));
    }

    private void HandleClick(System.Action onJoinClicked)
    {
        clickCount++;
        if (clickCount == 1)
        {
            lastClickTime = Time.time;
        }

        if (clickCount > 1 && Time.time - lastClickTime < doubleClickTime)
        {
            onJoinClicked(); // Call the action on double click
            clickCount = 0; // reset click count
        }
        else if (clickCount > 2 || Time.time - lastClickTime > doubleClickTime)
        {
            clickCount = 1; // reset click count if too much time has passed
            lastClickTime = Time.time;
        }
    }

    private void OnDestroy()
    {
        joinButton.onClick.RemoveAllListeners();
    }
}