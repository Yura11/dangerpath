using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Photon.Pun;
using System;

public class OnlinePositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;
    List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    // Start is called before the first frame update
    void Start()
    {
       
    }

    public void InitializeLeaderboard()
    {
        // Initialize the leaderboard with carLapCounters
       // leaderboardUIHandler.UpdateList(carLapCounters);
    }

 /*   public void FindPlayers()
    {
        CarLapCounter[] NewPlayers = FindObjectsOfType<CarLapCounter>();
        // Додайте ваш код тут, який ви хочете виконати після натискання пробілу
        Debug.Log(NewPlayers.Length);
        // Ваш код для обробки пробілу
        PhotonView[] NewPhoton = FindObjectsOfType<PhotonView>();
        Debug.Log(NewPhoton.Length);
        CarLapCounter[] carLapCounterArray = GameObject.FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList();

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            lapCounter.OnPassCheckpoint += OnPassCheckpoint;
        }

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
    }

    // This method is called when a checkpoint is passed
    void OnPassCheckpoint(CarLapCounter carLapCounter)
    {
        if (GameManager.Instance.GetGameState() == GameStates.raceOver)
            return;
        // Sort carLapCounters by the number of checkpoints passed and time at the last checkpoint
        carLapCounters = carLapCounters
            .OrderByDescending(s => s.GetNumberOfCheckpointsPassed())
            .ThenBy(s => s.GetTimeAtLastCheckPoint())
            .ToList();

        // Find the position of the passed carLapCounter
        int carPosition = carLapCounters.IndexOf(carLapCounter) + 1;

        // Set the car position
        carLapCounter.SetCarPosition(carPosition);

        // Update the leaderboard UI
        leaderboardUIHandler.UpdateList(carLapCounters);
    }*/
}