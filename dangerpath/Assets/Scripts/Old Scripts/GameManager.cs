
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

// ���������� ����� ���
public enum GameStates {countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    bool Leftloby=false;
    SpawnPlayers spawnPlayers;
    LeaderboardUIHandler leaderboardUIHandler;
    OnlinePositionHandler onlinePositionHandler;
    CountDownUIHandler countDownUIHandler;
    GameOverHandler gameOverHandler;
    WaitingUIHandler waitingUIHandler;
    CarLapCounter[] carLapCounters;
    private List<int> availableActorNumbers = new List<int>();

    // ��������� ��������� GameManager ��� ������� � ����� ������ ��������
    public static GameManager Instance = null;

    // �����, �� �������� �������� ���� ���
    GameStates gameState = GameStates.countDown;

    private void Awake()
    {
     //   PhotonNetwork.AutomaticallySyncScene = true;
        countDownUIHandler = FindObjectOfType<CountDownUIHandler>();
       // leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
       // onlinePositionHandler = FindObjectOfType<OnlinePositionHandler>();
     //   spawnPlayers = FindAnyObjectByType<SpawnPlayers>();
     //   gameOverHandler = FindAnyObjectByType<GameOverHandler>();
      //  waitingUIHandler = FindAnyObjectByType<WaitingUIHandler>();
        // ��������, �� ���� ��������� GameManager
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);  // �������� ��'����, ���� ����� ��������� ��� ����
            return;
        }

       // DontDestroyOnLoad(gameObject); // ������������ ������������ ����� ��'���� ��� ����������� ����� ����
      //  leaderboardUIHandler.gameObject.SetActive(false);
    }

    // ����������� ���
    void Start()
    {
        countDownUIHandler.CountDownStart();
    }

    void Update()
    {
        /* if (Input.GetKeyDown(KeyCode.Space))
         {
             Resetleaderboard();
         }*/

        if (Leftloby)
            Resetleaderboard();
    }
    // ����� ������ ����
    void LevelStart()
    {
       // if (/*PhotonNetwork.IsConnected*/)
      //  {
        //    gameState = GameStates.waiting; // ������������ ����� ��� � �����
       // }else
        {
            gameState= GameStates.running;
        }

        Debug.Log("Level Started");
    }

    // ������� ��� ��������� ��������� ����� ���
    public GameStates GetGameState()
    {
        return gameState;
    }

    public void PlayAgain()
    {
        TopDownCarController[] carControllers = FindObjectsOfType<TopDownCarController>();

        foreach (TopDownCarController carController in carControllers)
        {
            Destroy(carController.gameObject);
        }
      //  waitingUIHandler.gameObject.SetActive(true);
        countDownUIHandler.gameObject.SetActive(true);
        countDownUIHandler.countDownText.text = " ";
      //  leaderboardUIHandler.gameObject.SetActive(false) ;
    }

    // ����� ������ �����
    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");
     //   spawnPlayers.RecolorPlayersObjects();
     //   spawnPlayers.RenamePlayersObjects();
       // Resetleaderboard();
        carLapCounters = FindObjectsOfType<CarLapCounter>();
        gameState = GameStates.running; // ���� ����� ��� �� "running"
    }


    public void OnRaceEndForMe()
    {
        bool allRaceComplete = true;

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            if (!lapCounter.isRaceComplete())
            {
                allRaceComplete = false;
                break;  // ���� ��� ���� lapCounter.isRaceComplete() �� � true, �������� � �����
            }
        }

        if (allRaceComplete)
        {
            OnRaceEnd();
        }
    }

    public void OnRaceEnd()
    {
        gameOverHandler.gameObject.SetActive(true);
        gameOverHandler.SetWinnersPosition();
        gameState = GameStates.raceOver;
    }

    // ϳ���� �� ���� ������������ �����
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
      //  PhotonNetwork.AddCallbackTarget(this);
    }

    // �����, �� ���������� ��� ����������� ���� �����
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart(); // ������ ������ LevelStart
    }

//    public override void OnPlayerLeftRoom(Player otherPlayer)
  //  {
  //      Leftloby = true;
 //   }

    private void Resetleaderboard()
    {
       // onlinePositionHandler.FindPlayers();
      //  leaderboardUIHandler.CreateLeaderBoardItems();
        onlinePositionHandler.InitializeLeaderboard();
        Debug.Log("List Updated");
        Leftloby = false;
    }
}
