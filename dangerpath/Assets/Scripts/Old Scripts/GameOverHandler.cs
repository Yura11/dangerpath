using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GameOverHandler : MonoBehaviour
{
    public Text firstPlaceText;
    public Text secondPlaceText;
    public Text thirdPlaceText;
    public GameObject GameOverCanvas;

    CarLapCounter[] carLapCounters;
    GameManager gameManager;

    List<float> lapTimes = new List<float>();

    LeaderboardUIHandler leaderboardUIHandler;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetWinnersPosition();
        }
    }

    public void SetWinnersPosition()
    {
        gameManager = FindObjectOfType<GameManager>();
        leaderboardUIHandler = FindObjectOfType<LeaderboardUIHandler>();
        GameOverCanvas.gameObject.SetActive(true);
        BestTimes();
       // firstPlaceText.text = leaderboardUIHandler.GetWinners(0) + "\n з часом " + Math.Round(lapTimes[0], 2).ToString("F2");
      //  secondPlaceText.text = leaderboardUIHandler.GetWinners(1) + "\n з часом " + Math.Round(lapTimes[1], 2).ToString("F2");
      //S  thirdPlaceText.text = leaderboardUIHandler.GetWinners(2) + "\n з часом " + Math.Round(lapTimes[2], 2).ToString("F2");
    }

    public void OnButtonPlayAgain()
    {
        gameManager.PlayAgain();
        gameObject.SetActive(false);
    }

    private void BestTimes()
    {
        carLapCounters = FindObjectsOfType<CarLapCounter>();
        lapTimes.Clear(); // Clear the list before adding new lap times

        foreach (CarLapCounter lapCounter in carLapCounters)
        {
            lapTimes.Add(lapCounter.GetTimeAtLastCheckPoint());
        }
        lapTimes.Sort();
    }
}
