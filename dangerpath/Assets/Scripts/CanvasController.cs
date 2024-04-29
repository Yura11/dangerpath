using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CustomNetworkManager networkManager;
    public GameObject lobbyListItemPrefab;
    public Transform lobbyListParent;
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        CustomNetworkManager.OnLobbiesUpdated += UpdateLobbyList;
    }

    private void OnDisable()
    {
        CustomNetworkManager.OnLobbiesUpdated -= UpdateLobbyList;
    }

    public void UpdateLobbyList(List<GameRoom> lobbies)
    {
        // Clear existing list
        foreach (Transform child in lobbyListParent)
        {
            Destroy(child.gameObject);
        }

        // Populate new list items
        foreach (var lobby in lobbies)
        {
            GameObject item = Instantiate(lobbyListItemPrefab, lobbyListParent);
            item.GetComponent<LobbyListItem>().Setup(lobby.RoomName, lobby.MaxPlayers, () => RequestJoinRoom(lobby.RoomName));
        }
    }

    public void RequestJoinRoom(string roomName)
    {
        JoinRoomRequest request = new JoinRoomRequest { roomName = roomName };
        NetworkClient.Send(request);
    }


    public void OnCreateRoomButtonClicked()
    {
        CreateRoomRequest request = new CreateRoomRequest
        {
            roomName = "test",
            maxPlayers = 8
        };

        NetworkClient.Send(request);
    }

    public void OnRefreshLobbiesButtonClicked()
    {
        networkManager.RequestLobbyListUpdate();
    }

    private void OnLobbyListReceived(LobbyListMessage message)
    {
        UpdateLobbyList(message.Lobbies);
    }
}
