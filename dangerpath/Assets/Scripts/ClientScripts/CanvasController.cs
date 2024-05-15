using Mirror;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public CustomNetworkManager networkManager;
    public GameObject lobbyListItemPrefab;
    public GameObject createLobbyMenu;
    public GameObject lobby;
    public GameObject lobbyList;
    public GameObject playersListItemPrefab;
    public GameObject readyBTN;
    public GameObject notReadyBTN;
    public GameObject startGameBTN;
    public Transform lobbyListParent;
    public Transform playersListParent;
    public Text ErrorLobbyText;
    public static UIManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
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

    public void GetPlayersCarDate(Guid RoomId, string Message, bool GameAbleToStart)
    {
        if (GameAbleToStart)
        {
            PlayersChosenCarData request = new PlayersChosenCarData { RoomId = RoomId, CarId = LobbyCarListUI.GetIdOfChosenCar() };
            NetworkClient.Send(request);
        } 
        else 
        {
            ErrorLobbyText.gameObject.SetActive(true);
            ErrorLobbyText.text = Message;
        }
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
            item.GetComponent<LobbyListItem>().SetupLobbyListItem(lobby.RoomName, lobby.MaxPlayers, lobby.Players.Count, lobby.MapNumber, lobby.GameState, () => RequestJoinRoom(lobby.RoomName));
        }
    }

    public void UpdatePlayersList(List<NetworkPlayer> lobbyPlayers)
    {
        foreach (Transform cild in playersListParent)
        {
            Destroy(cild.gameObject);
        }
        foreach (var player in lobbyPlayers)
        {
            GameObject item = Instantiate(playersListItemPrefab, playersListParent);
            item.GetComponent<PlayersListItem>().SetupPlayersListItem(player.playerName, player.playerReadyStatus, player.isOwner);
        }
    }

    public void RequestJoinRoom(string roomName)
    {
        JoinRoomRequest request = new JoinRoomRequest { roomName = roomName, playerName= CrossScaneInfoHolder.GamerNickName };
        NetworkClient.Send(request);
    }

    public void RequestPlayerStatus()
    {
        PlayerStatusRequest request = new PlayerStatusRequest { };
        NetworkClient.Send(request);
    }

    public void SetNotReadyAndStartBTNs(bool isOwner)
    {
        readyBTN.SetActive(!isOwner);
        notReadyBTN.SetActive(false);
        startGameBTN.SetActive(isOwner);
    }

    public void UpdateUIOnRoomJoin()
    {
        if (lobbyList != null && lobbyList.activeSelf)
            lobbyList.SetActive(false);
        if (createLobbyMenu != null && createLobbyMenu.activeSelf)
            createLobbyMenu.SetActive(false);
        if (lobby != null && !lobby.activeSelf)
            lobby.SetActive(true);
        RequestPlayerStatus();
    }

    public void UpdateUIOnRoomLeave()
    {
        if (!lobbyList.activeSelf)
            lobbyList.SetActive(true);
        if (!createLobbyMenu.activeSelf)
            createLobbyMenu.SetActive(false);
        if (lobby.activeSelf)
            lobby.SetActive(false);
    }

    public void OnCreateRoomButtonClicked()
    {
        CreateRoomRequest request = new CreateRoomRequest
        {
            roomName =  $"{CrossScaneInfoHolder.GamerNickName}"+"'s room",
            mapNumber = CreateLobbyUImanager.GetMapNumberInLobby(),
            maxPlayers = CreateLobbyUImanager.GetNumberOfPlayersInLobby(),
            numberOfLaps = CreateLobbyUImanager.GetNumberOfLapsInLobby(),
            nickName = CrossScaneInfoHolder.GamerNickName,
        };
        NetworkClient.Send(request);
    }

    public void OnLeaveRoomButtonClicked()
    {
        if (!NetworkClient.isConnected)
        {
            Debug.Log("Client is not connected to the server.");
            return;
        }

        LeaveRoomRequest request = new LeaveRoomRequest
        {
            roomName = "test" // You need to dynamically set this based on the room the player is currently in
        };
        NetworkClient.Send(request);
        Debug.Log("LeaveRoomRequest send.");
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

    public void OnReadyBTNClicked()
    {
        SetPlayerReadyStatusRequest request = new SetPlayerReadyStatusRequest
        {
            PlayerReadyStatus = true,
        };
        NetworkClient.Send(request);
        readyBTN.SetActive(false);
        notReadyBTN.SetActive(true);
        startGameBTN.SetActive(false);
    }

    public void OnNotReadyBTNClicked()
    {
        SetPlayerReadyStatusRequest request = new SetPlayerReadyStatusRequest
        {
            PlayerReadyStatus = false,
        };
        NetworkClient.Send(request);
        readyBTN.SetActive(true);
        notReadyBTN.SetActive(false);
        startGameBTN.SetActive(false);
    }

    public void OnStartGameBTNClicked()
    {
        ErrorLobbyText.gameObject.SetActive(false);
        StartGameRequest request = new StartGameRequest { };
        NetworkClient.Send(request);
    }

    private void OnLobbyListReceived(LobbyListMessage message)
    {
        Debug.Log($"Received lobby list with {message.Lobbies.Count} lobbies.");
        UpdateLobbyList(message.Lobbies);
    }
}
