using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using System.Threading;

public enum GameStates { countDown, running, raceOver };

public class GameManager : MonoBehaviour
{
    bool Leftloby = false;
    SpawnPlayers spawnPlayers;
    LeaderboardUIHandler leaderboardUIHandler;
    OnlinePositionHandler onlinePositionHandler;
    CountDownUIHandler countDownUIHandler;
    GameOverHandler gameOverHandler;
    WaitingUIHandler waitingUIHandler;
    CarLapCounter[] carLapCounters;
    private List<int> availableActorNumbers = new List<int>();

    public static GameManager Instance = null;

    public static GameStates gameState = GameStates.countDown;

    private void Awake()
    {
        SetAllCollisionsEnabled(false);
        countDownUIHandler = FindObjectOfType<CountDownUIHandler>();

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        countDownUIHandler.CountDownStart();
    }

    void Update()
    {
        if (Leftloby)
            Resetleaderboard();
    }

    void LevelStart()
    {
        gameState = GameStates.countDown;
        Debug.Log("Level Started");
    }

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
    }

    public void OnRaceStart()
    {
        Debug.Log("OnRaceStart");
        SetAllCollisionsEnabled(true);
        carLapCounters = FindObjectsOfType<CarLapCounter>();
        MyCinemachine.Instance.SetCamera();
        gameState = GameStates.running;
    }

    public void OnRaceEndForMe()
    {
        bool allRaceComplete = true;

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            if (!lapCounter.isRaceComplete())
            {
                allRaceComplete = false;
                break;
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

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LevelStart();
    }

    private void Resetleaderboard()
    {
        onlinePositionHandler.InitializeLeaderboard();
        Debug.Log("List Updated");
        Leftloby = false;
    }

    public static void SetAllCollisionsEnabled(bool enabled)
    {
        // Find all 2D colliders in the scene
        Collider2D[] colliders2D = FindObjectsOfType<PolygonCollider2D>();
        foreach (var collider in colliders2D)
        {
            collider.enabled = enabled;
        }

        // Find all 3D colliders in the scene
        Collider[] colliders3D = FindObjectsOfType<Collider>();
        foreach (var collider in colliders3D)
        {
            collider.enabled = enabled;
        }

        Debug.Log("All colliders set to: " + enabled);
    }

    public void SortPlayerArry(GameRoom room)
    {

    }
}
