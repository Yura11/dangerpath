using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Diagnostics;
using Debug = UnityEngine.Debug;
using JetBrains.Annotations;

public class CarLapCounter : MonoBehaviour
{
    GameManager gameManager;
    TopDownCarController topDownCarController;

    public Text carPositionText;

    private Stopwatch timer;

    int passedCheckPointNumber = 0;
    float timeAtLastPassedCheckPoint = 0;

    int numberOfPassedCheckpoints = 0;

    int lapsCompleted = 0;
    const int lapsToComplete = 1;

    bool isRaceCompleted = false;

    int carPosition = 1;

    bool isHideRoutineRunning = false;
    float hideUIDelayTime;

    public event Action<CarLapCounter> OnPassCheckpoint;

    public static bool ReremoveConrol=false;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
    }
    public void SetCarPosition(int position)
    {
        carPosition = position;
    }

    public bool isRaceComplete()
    {
        return isRaceCompleted;
    }

    public int GetNumberOfCheckpointsPassed()
    {
        return numberOfPassedCheckpoints;
    }

    public float GetTimeAtLastCheckPoint()
    {
        return timeAtLastPassedCheckPoint;
    }

    // Початок корутини для відображення позиції гравця
    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime += delayUntilHidePosition;

        carPositionText.text = carPosition.ToString();

        carPositionText.gameObject.SetActive(true);

        if (!isHideRoutineRunning)
        {
            isHideRoutineRunning = true;
            yield return new WaitForSeconds(hideUIDelayTime);
            carPositionText.gameObject.SetActive(false);

            isHideRoutineRunning = false;
        }
    }

    // Обробка зіткнення з чекпоінтом
    void OnTriggerEnter2D(Collider2D collider2D)
    {
        if(!PhotonNetwork.IsConnected)
        {
            return;
        }
        if (collider2D.CompareTag("CheckPoints"))
        {
            if (timer == null )
            {
                timer = new Stopwatch();
                timer.Start();
                Debug.Log("timerStarted");
            }
            if (isRaceCompleted)
                return;
            CheckPoint checkPoint = collider2D.GetComponent<CheckPoint>();

            if (passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                passedCheckPointNumber = checkPoint.checkPointNumber;

                numberOfPassedCheckpoints++;

                timeAtLastPassedCheckPoint = (float)timer.Elapsed.TotalSeconds;

                OnPassCheckpoint?.Invoke(this);

                //timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    passedCheckPointNumber = 0;
                    lapsCompleted++;

                    if (lapsCompleted >= lapsToComplete)
                    {
                        isRaceCompleted = true;
                        gameManager.OnRaceEndForMe();
                    }
                }

                if (isRaceCompleted)
                    StartCoroutine(ShowPositionCO(100));
                else StartCoroutine(ShowPositionCO(1.0f));
            }
        }
    }

    //internal void SetPlayer(Player player)
    //{
    //  throw new NotImplementedException();
    //}
}
