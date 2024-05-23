
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

// Визначення станів гри
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

    // Статичний екземпляр GameManager для доступу з інших частин програми
    public static GameManager Instance = null;

    // Змінна, що відображає поточний стан гри
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
        // Перевірка, чи існує екземпляр GameManager
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);  // Знищення об'єкта, якщо інший екземпляр вже існує
            return;
        }

       // DontDestroyOnLoad(gameObject); // Забезпечення незнищенності цього об'єкта при завантаженні нових сцен
      //  leaderboardUIHandler.gameObject.SetActive(false);
    }

    // Ініціалізація гри
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
    // Логіка старту рівня
    void LevelStart()
    {
       // if (/*PhotonNetwork.IsConnected*/)
      //  {
        //    gameState = GameStates.waiting; // Встановлення стану гри в режим
       // }else
        {
            gameState= GameStates.running;
        }

        Debug.Log("Level Started");
    }

    // Функція для отримання поточного стану гри
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

    // Логіка старту гонки
    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");
     //   spawnPlayers.RecolorPlayersObjects();
     //   spawnPlayers.RenamePlayersObjects();
       // Resetleaderboard();
        carLapCounters = FindObjectsOfType<CarLapCounter>();
        gameState = GameStates.running; // Зміна стану гри на "running"
    }


    public void OnRaceEndForMe()
    {
        bool allRaceComplete = true;

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            if (!lapCounter.isRaceComplete())
            {
                allRaceComplete = false;
                break;  // Якщо хоч один lapCounter.isRaceComplete() не є true, виходимо з циклу
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

    // Підпис на подію завантаження сцени
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
      //  PhotonNetwork.AddCallbackTarget(this);
    }

    // Логіка, що виконується при завантаженні нової сцени
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart(); // Виклик методу LevelStart
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
