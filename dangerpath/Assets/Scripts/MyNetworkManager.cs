using Mirror;
using UnityEngine;
using System.Collections.Generic;
using System;
using UnityEngine.SceneManagement;
using System.Linq;
using UnityEditor.MemoryProfiler;

public class CustomNetworkManager : NetworkManager
{
    public bool autoStartServer = false;

    private GameRoomManager gameRoomManager;
    public static event Action<List<GameRoom>> OnLobbiesUpdated;
    public static Dictionary<Guid, GameRoom> openMatches = new Dictionary<Guid, GameRoom>();
    public static Dictionary<Guid, HashSet<NetworkConnectionToClient>> matchConnections = new Dictionary<Guid, HashSet<NetworkConnectionToClient>>();
    public static List<NetworkConnectionToClient> waitingConnections = new List<NetworkConnectionToClient>();

    public override void Awake()
    {
        base.Awake();
        if (autoStartServer)
        {
            StartServer(); // Automatically start the server if autoStartServer is true
        }
    }

    public void CreateRoom(string roomName, int maxPlayers, int mapNumber, int numberOfLaps, NetworkConnection conn, string nickname)
    {
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("CreateRoom called with a non-client connection.");
            return;
        }

        Guid newRoomId = Guid.NewGuid();
        GameRoom newRoom = new GameRoom
        {
            RoomId = newRoomId,
            RoomName = roomName,
            MaxPlayers = maxPlayers,
            MapNumber = mapNumber,
            NumberOfLaps = numberOfLaps,
            Players = new List<NetworkPlayer>()
        };

        openMatches.Add(newRoomId, newRoom);
        if (!matchConnections.ContainsKey(newRoomId))
        {
            matchConnections.Add(newRoomId, new HashSet<NetworkConnectionToClient>());
        }
        matchConnections[newRoomId].Add(clientConn);

        gameRoomManager.AddPlayerToRoom(newRoom, clientConn,  nickname, true);
        gameRoomManager.NotifyRoomClientsOfUpdatedPlayerList(newRoomId);
    }

    public void SendRoomUpdate(GameRoom room)
    {
        RoomUpdateMessage updateMessage = new RoomUpdateMessage()
        {
            RoomId = room.RoomId,
            RoomName = room.RoomName,
            MaxPlayers = room.MaxPlayers,
            CurrentPlayers = room.Players.Count,
            MapNumber = room.MapNumber,
            NumberOfLaps = room.NumberOfLaps
        };
        NetworkServer.SendToAll(updateMessage);
    }

    // Invoke this method to update all clients with the latest list of game rooms
    public void UpdateLobbies()
    {
        var allRooms = openMatches.Values.ToList();
        LobbyListMessage response = new LobbyListMessage { Lobbies = allRooms };
        // Send the response to all connected clients, not just those waiting
        foreach (var conn in NetworkServer.connections.Values)
        {
            if (conn.isReady) // Make sure the connection is ready
                conn.Send(response);
        }
        OnLobbiesUpdated?.Invoke(allRooms);
        Debug.Log("Updated lobbies and notified all subscribers.");
    }

    public void OnServerConnect(NetworkConnection conn)
    {
        NetworkConnectionToClient clientConn = conn as NetworkConnectionToClient;
        base.OnServerConnect(clientConn);
        // Send the current lobby list to just the newly connected client
        var allRooms = openMatches.Values.ToList();
        LobbyListMessage response = new LobbyListMessage { Lobbies = allRooms };
        conn.Send(response);
    }

    private void UpdatePlayerData(ref NetworkPlayer targetPlayer, NetworkPlayer newData)
    {
        targetPlayer.playerName = newData.playerName;
        targetPlayer.isOwner = newData.isOwner;
        targetPlayer.connectionId = newData.connectionId;
        targetPlayer.playerReadyStatus = newData.playerReadyStatus;
        targetPlayer.carId = newData.carId;
    }

    #region Delete in future 

    public void LogPlayerNicknames(GameRoom room)
    {
        // Check if the room has any players
        if (room.Players.Count == 0)
        {
            Debug.Log("No players in the room.");
            return;
        }

        Debug.Log("Listing all player nicknames in the room:");
        foreach (NetworkPlayer player in room.Players)
        {
            Debug.Log(player.playerName); // Assuming playerName is the attribute storing the nickname
        }
    }

    private void OnLogPlayerNicknamesRequestReceived(NetworkConnection conn, LogPlayerNicknamesRequest data)
    {
        if (conn is NetworkConnectionToClient clientConn)
        {
            // «находимо к≥мнату, де знаходитьс€ гравець за `connectionId`.
            foreach (var room in CustomNetworkManager.openMatches.Values)
            {
                var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

                // якщо гравець знайдений, виводимо ≥нформац≥ю про к≥мнату.
                if (player != null)
                {
                    LogPlayerNicknames(room);
                    return;
                }
            }

            // якщо гравц€ з таким connectionId не знайдено.
            Debug.LogError($"Player not found with connection ID: {clientConn.connectionId}");
        }
        else
        {
            Debug.LogError("Received log player nicknames request from a non-client connection.");
        }
    }
    #endregion

    #region Server System Callbacks
    public override void OnStartServer()
    {
        base.OnStartServer();
        gameRoomManager = new GameRoomManager(new List<GameRoom>(), this); // Ensure this is being called.
        NetworkServer.RegisterHandler<LeaveRoomRequest>(OnLeaveRoomRequestReceived);
        NetworkServer.RegisterHandler<RequestLobbyListMessage>(OnRequestLobbyListReceived);
        NetworkServer.RegisterHandler<CreateRoomRequest>(OnCreateRoomRequestReceived);
        NetworkServer.RegisterHandler<JoinRoomRequest>(OnJoinRoomRequestReceived);
        NetworkServer.RegisterHandler<PlayerStatusRequest>(OnPlayerStatusRequestReceived);
        NetworkServer.RegisterHandler<SetPlayerReadyStatusRequest>(OnSetPlayerReadyStatusRequestReceived);
        NetworkServer.RegisterHandler<StartGameRequest>(OnStartGameRequestReceived);
        NetworkServer.RegisterHandler<PlayersChosenCarData>(OnPlayersChosenCarDataReceived);
        NetworkServer.RegisterHandler<SpawnPlayerRequest>(OnSpawnPlayerRequestReceived);
        NetworkServer.RegisterHandler<OnPlayerPassedChecpoint>(OnPlayerPassedChecpointReceived);
        #region Delete in future 
        NetworkServer.RegisterHandler<LogPlayerNicknamesRequest>(OnLogPlayerNicknamesRequestReceived);
        #endregion
    }

    private void OnRequestLobbyListReceived(NetworkConnection conn, RequestLobbyListMessage message)
    {
        UpdateLobbies();
    }

    private void OnCreateRoomRequestReceived(NetworkConnection conn, CreateRoomRequest request)
    {
        if (openMatches.Any(r => r.Value.RoomName == request.roomName))
        {
            conn.Send(new CreateRoomResponse { Success = false, Message = "Room already exists." });
        }
        else
        {
            CreateRoom(request.roomName, request.maxPlayers, request.mapNumber, request.numberOfLaps, conn, request.nickName);
            conn.Send(new CreateRoomResponse { Success = true, Message = "Room created successfully." });
        }
    }

    private void OnJoinRoomRequestReceived(NetworkConnection conn, JoinRoomRequest request)
    {
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("JoinRoomRequest received from a non-client connection.");
            return;
        }

        var room = openMatches.Values.FirstOrDefault(r => r.RoomName == request.roomName);
        if (!room.Equals(default(GameRoom)) && room.Players.Count < room.MaxPlayers)
        {
            gameRoomManager.AddPlayerToRoom(room, clientConn, request.playerName, false);

            // Update player list for this lobby only
            gameRoomManager.NotifyRoomClientsOfUpdatedPlayerList(room.RoomId);

            conn.Send(new JoinRoomResponse { success = true, message = "Successfully joined the room." });
            UpdateLobbies();
            gameRoomManager.NotifyRoomClientsOfUpdatedPlayerList(room.RoomId);
        }
        else
        {
            conn.Send(new JoinRoomResponse { success = false, message = "Failed to join room: Room is full or does not exist." });
        }
    }

    private void OnLeaveRoomRequestReceived(NetworkConnection conn, LeaveRoomRequest request)
    {
        var clientConn = conn as NetworkConnectionToClient;
        if (clientConn == null)
        {
            Debug.LogError("Received leave room request from a non-client connection.");
            return;
        }

        var room = openMatches.Values.FirstOrDefault(r => r.RoomName == request.roomName);
        if (room.Equals(default(GameRoom)))
        {
            Debug.LogError("Room not found.");
            return;
        }

        var playerToRemove = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());
        if (playerToRemove != null)
        {
            room.Players.Remove(playerToRemove);
            openMatches[room.RoomId] = room;

            // Update player list for this lobby only
            gameRoomManager.NotifyRoomClientsOfUpdatedPlayerList(room.RoomId);

            conn.Send(new LeaveRoomResponse
            {
                success = true,
                message = $"Successfully left the room. Player: {playerToRemove.playerName}",
                playerName = playerToRemove.playerName
            });
            if (playerToRemove.isOwner)
            {
                openMatches.Remove(room.RoomId);
                BroadcastLobbyLeaft updateMessage = new BroadcastLobbyLeaft()
                {
                };
                NetworkServer.SendToAll(updateMessage);
            }
            UpdateLobbies();
        }
        else
        {
            conn.Send(new LeaveRoomResponse
            {
                success = false,
                message = "Player not found in the room based on connection ID."
            });
            Debug.LogError("Failed to find player in room based on connection ID.");
        }
    }

    private void OnPlayerStatusRequestReceived(NetworkConnection conn, PlayerStatusRequest request)
    {
        // We check whether `conn` is an object of type NetworkConnectionToClient.
        if (conn is NetworkConnectionToClient clientConn)
        {
            // We go through all active matches to find a player by his connectionId.
            foreach (var room in CustomNetworkManager.openMatches.Values)
            {
                var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

                // If the player is found, we send the status.
                if (player != null)
                {
                    PlayerStatusMessage message = new PlayerStatusMessage()
                    {
                        OwnerStatus = player.isOwner,
                    };
                    conn.Send(message);
                    return;
                }
            }

            // If no player with matching connectionId is found.
            Debug.LogError($"Player not found with connection ID: {clientConn.connectionId}");
        }
        else
        {
            Debug.LogError("Received player status request from a non-client connection.");
        }
    }

    private void OnSetPlayerReadyStatusRequestReceived(NetworkConnection conn, SetPlayerReadyStatusRequest request)
    {
        Debug.Log("SetPlayerReadyStatusRequestReceived");
        // We check whether `conn` is an object of type NetworkConnectionToClient.
        if (conn is NetworkConnectionToClient clientConn)
        {
            // «находимо к≥мнату, де знаходитьс€ гравець за `connectionId`.
            foreach (var kvp in CustomNetworkManager.openMatches)
            {
                var room = kvp.Value;

                // We are looking for a player by `connectionId` among all players in the room.
                var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

                // If the player is found, we update the readiness status.
                if (player != null)
                {
                    player.playerReadyStatus = request.PlayerReadyStatus;

                    // We update the state of the room in the dictionary so that the changes are saved.
                    CustomNetworkManager.openMatches[room.RoomId] = room;

                    // We notify all customers in the room about the update of the player list.
                    gameRoomManager.NotifyRoomClientsOfUpdatedPlayerList(room.RoomId);
                    return;
                }
            }

            // If no player with matching `connectionId` is found.
            Debug.LogError($"Player not found with connection ID: {clientConn.connectionId}");
        }
        else
        {
            Debug.LogError("Received set player ready status request from a non-client connection.");
        }
    }

    private void OnStartGameRequestReceived(NetworkConnection conn, StartGameRequest request)
    {
        Debug.Log("Start Game Request Received");
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("StartGameRequest received from a non-client connection.");
            return;
        }

        // «находимо к≥мнату, де знаходитьс€ гравець за `connectionId`.
        foreach (var room in CustomNetworkManager.openMatches.Values)
        {
            var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

            // якщо гравець знайдений у к≥мнат≥
            if (player != null)
            {
                // ѕерев≥р€Їмо, чи вс≥ гравц≥ в лобб≥ готов≥
                bool allPlayersReady = room.Players.All(p => p.playerReadyStatus);

                if (allPlayersReady)
                {
                    // якщо вс≥ гравц≥ готов≥, в≥дправл€Їмо пов≥домленн€ про початок гри вс≥м кл≥Їнтам у лобб≥
                    StartGameResponse startGameResponse = new StartGameResponse
                    {
                        GameAbleToStart = true,
                        RoomId = room.RoomId
                    };

                    // ЌадсилаЇмо пов≥домленн€ вс≥м гравц€м у к≥мнат≥
                    foreach (var roomPlayer in room.Players)
                    {
                        var playerConnection = NetworkServer.connections.FirstOrDefault(conn => conn.Value.connectionId.ToString() == roomPlayer.connectionId).Value;
                        if (playerConnection != null)
                        {
                            playerConnection.Send(startGameResponse);
                        }
                        else
                        {
                            Debug.LogError($"Connection for player with ID {roomPlayer.connectionId} not found.");
                        }
                    }

                    Debug.Log("Game started in room: " + room.RoomName);
                }
                else
                {
                    // якщо не вс≥ гравц≥ готов≥, в≥дправл€Їмо в≥дпов≥дь т≥льки запитувачу
                    clientConn.Send(new StartGameResponse { GameAbleToStart = false, Message = "Not all players are ready." });
                    Debug.Log("Not all players are ready in room: " + room.RoomName);
                }
                return; // «ак≥нчуЇмо обробку запиту
            }
        }

        // якщо гравець не знайдений у к≥мнат≥
        Debug.LogError($"Player with connection ID {clientConn.connectionId} not found in any room.");
        conn.Send(new StartGameResponse { GameAbleToStart = false, Message = "You are not in any room." });
    }

    private void OnPlayersChosenCarDataReceived(NetworkConnection conn, PlayersChosenCarData data)
    {
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("PlayersChosenCarData received from a non-client connection.");
            return;
        }

        // «находимо к≥мнату за RoomId
        if (CustomNetworkManager.openMatches.TryGetValue(data.RoomId, out GameRoom room))
        {
            // «находимо гравц€ в к≥мнат≥ за connectionId
            var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

            if (player != null)
            {
                // «м≥нюЇмо carId гравц€
                player.carId = data.CarId;
                room.GameState = true;

                // ќновлюЇмо стан гри
                CustomNetworkManager.openMatches[data.RoomId] = room;

                // ЌадсилаЇмо в≥дпов≥дь
                PlayersChosenCarDataResponse response = new PlayersChosenCarDataResponse()
                {
                    MapNumber = room.MapNumber,
                    PlayerList = room.Players,
                };
                clientConn.Send(response);
            }
            else
            {
                Debug.LogError($"Player with connection ID {clientConn.connectionId} not found in room {data.RoomId}.");
            }
        }
        else
        {
            Debug.LogError($"Room with ID {data.RoomId} not found.");
        }
    }

    private void OnSpawnPlayerRequestReceived(NetworkConnection conn, SpawnPlayerRequest request) 
    {
        if (!(conn is NetworkConnectionToClient clientConn))
        {
            Debug.LogError("Request received from a non-client connection.");
            return;
        }
        foreach (var room in CustomNetworkManager.openMatches.Values)
        {
            var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

            // якщо гравець знайдений у к≥мнат≥
            if (player != null)
            {
                PlayerSpawner.Instance.SpawnPlayers(room);
            }
        }
    }

    private void OnPlayerPassedChecpointReceived(NetworkConnection conn, OnPlayerPassedChecpoint data)
    {
        if (conn is NetworkConnectionToClient clientConn)
        {
            // Find the room where the player is located based on the connection ID.
            foreach (var kvp in CustomNetworkManager.openMatches)
            {
                var room = kvp.Value;

                // Find the player by connectionId among all players in the room.
                var player = room.Players.FirstOrDefault(p => p.connectionId == clientConn.connectionId.ToString());

                // If the player is found, update the player's data with the received data.
                if (player != null)
                {
                    UpdatePlayerData(ref player, data.player);
                    Debug.Log($"Player data updated for {player.playerName} in room {room.RoomId}");
                    break; // Exit the loop once the player is found and updated.
                }
            }
        }
    }

    #endregion

                    #region Client System Callbacks
    public override void OnStartClient()
    {
        base.OnStartClient();
        NetworkClient.RegisterHandler<LeaveRoomResponse>(OnLeaveRoomResponseReceived);
        NetworkClient.RegisterHandler<LobbyListMessage>(OnLobbyListReceived);
        NetworkClient.RegisterHandler<CreateRoomResponse>(OnCreateRoomResponseReceived);
        NetworkClient.RegisterHandler<JoinRoomResponse>(OnJoinRoomResponseReceived);
        NetworkClient.RegisterHandler<BroadcastLobbyLeaft>(OnBroadcastLobbyLeaftReceived);
        NetworkClient.RegisterHandler<PlayerListUpdateMessage>(OnPlayerListUpdateReceived);
        NetworkClient.RegisterHandler<PlayerStatusMessage>(OnPlayerStatusMessageReceived);
        NetworkClient.RegisterHandler<StartGameResponse>(OnStartGameResponseReceived);
        NetworkClient.RegisterHandler<PlayersChosenCarDataResponse>(OnPlayersChosenCarDataResponseReceived);
    }

    private void OnLobbyListReceived(LobbyListMessage message)
    {
        Debug.Log($"Received lobby list with {message.Lobbies.Count} lobbies.");
        UIManager.Instance.UpdateLobbyList(message.Lobbies);
    }

    private void OnCreateRoomResponseReceived(CreateRoomResponse response)
    {
        Debug.Log($"Create Room Response: {response.Message}");
        if (response.Success)
        {
            Debug.Log("Room created successfully.");
            UIManager.Instance.UpdateUIOnRoomJoin();
        }
    }

    private void OnJoinRoomResponseReceived(JoinRoomResponse response)
    {
        Debug.Log($"Join Room Response: {response.message}");
        if (response.success)
        {
            Debug.Log("Successfully joined the room.");
        }
        UIManager.Instance.UpdateUIOnRoomJoin();
    }

    private void OnLeaveRoomResponseReceived(LeaveRoomResponse response)
    {
        Debug.Log(response.message);
        if (response.success)
        {
            UIManager.Instance.UpdateUIOnRoomLeave();
            UpdateLobbies();
        }
        else
        {
            Debug.LogError("Failed to leave room: " + response.message);
            // Handle error (maybe show a message to the user)
        }
    }

    private void OnBroadcastLobbyLeaftReceived(BroadcastLobbyLeaft response)
    {
        UIManager.Instance.UpdateUIOnRoomLeave();
        UpdateLobbies();
    }

    public void OnPlayerListUpdateReceived(PlayerListUpdateMessage message)
    {
        Debug.Log("PlayerListUpdateReceived");
        var playerDataList = message.Players.Select(pd => new NetworkPlayer
        {
            playerName = pd.PlayerName,
            playerReadyStatus = pd.ReadyStatus,
            isOwner = pd.OwnerStatus,
        }).ToList();

        // Update player list display in UI
        UIManager.Instance.UpdatePlayersList(playerDataList);
    }

    private void OnPlayerStatusMessageReceived(PlayerStatusMessage message)
    {
        UIManager.Instance.SetNotReadyAndStartBTNs(message.OwnerStatus);
    }

    private void OnStartGameResponseReceived(StartGameResponse response)
    {
        UIManager.Instance.GetPlayersCarDate(response.RoomId, response.Message, response.GameAbleToStart);
    }


    void Start()
    {
        NetworkClient.RegisterHandler<PlayerListUpdateMessage>(OnPlayerListUpdateReceived);
    }

    public void OnPlayersChosenCarDataResponseReceived(PlayersChosenCarDataResponse response)
    {
        //NetworkManager manager = NetworkManager.singleton;
        //manager.ServerChangeScene("Game");
        CrossScaneInfoHolder.PlayerList = response.PlayerList;
        CrossScaneInfoHolder.MapNumber = response.MapNumber;
        SceneManager.LoadScene("Game");
        bool isOwner = CrossScaneInfoHolder.PlayerList
        .Find(player => player.playerName == CrossScaneInfoHolder.GamerNickName)?.isOwner ?? false;

        if (isOwner)
        {
            // ¬≥дправл€Їмо запит на спавн гравц€
            SpawnPlayerRequest request = new SpawnPlayerRequest { };
            NetworkClient.Send(request);
        }
    }

    #endregion
}
