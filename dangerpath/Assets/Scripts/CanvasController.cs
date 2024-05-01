using Mirror;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CustomNetworkManager networkManager;
    public GameObject lobbyListItemPrefab;
    public GameObject createLobbyMenu;
    public GameObject lobby;
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

    void Start()
    {
        CustomNetworkManager.OnLobbiesUpdated += UpdateLobbyList;
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
        foreach (Transform child in lobbyListParent)
        {
            Destroy(child.gameObject);
        }

        foreach (var lobby in lobbies)
        {
            GameObject item = Instantiate(lobbyListItemPrefab, lobbyListParent);
            item.GetComponent<LobbyListItem>().Setup(lobby.RoomName, lobby.MaxPlayers, lobby.CurrentPlayers, () => RequestJoinRoom(lobby.RoomName));
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
            mapNumber = CreateLobbyUImanager.GetMapNumberInLobby(),
            maxPlayers = CreateLobbyUImanager.GetNumberOfPlayersInLobby(),
            numberOfLaps = CreateLobbyUImanager.GetNumberOfLapsInLobby(),
        };
        NetworkClient.Send(request);
    }

    public void OnRefreshLobbiesButtonClicked()
    {
        if (NetworkClient.isConnected)
        {
            NetworkClient.Send(new RequestLobbyListMessage());
            Debug.Log("Request for lobby list update sent.");
        }
        else
        {
            Debug.Log("Client is not connected to the server.");
        }
    }

    private void OnLobbyListReceived(LobbyListMessage message)
    {
        Debug.Log($"Received lobby list with {message.Lobbies.Count} lobbies.");
        UpdateLobbyList(message.Lobbies);
    }
}
