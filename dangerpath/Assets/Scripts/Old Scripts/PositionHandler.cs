using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PositionHandler : MonoBehaviour
{
    LeaderboardUIHandler leaderboardUIHandler;
    List<CarLapCounter> carLapCounters = new List<CarLapCounter>();

    private void Awake()
    {
        // Find all CarLapCounter components in the scene
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();
        carLapCounters = carLapCounterArray.ToList();

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
          //  lapCounter.OnPassCheckpoint += OnPassCheckpoint;
        }

        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the leaderboard with carLapCounters
        /*   leaderboardUIHandler.UpdateList(carLapCounters);
       }



           // This method is called when a checkpoint is passed
           void OnPassCheckpoint(CarLapCounter carLapCounter)
       {
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
}
