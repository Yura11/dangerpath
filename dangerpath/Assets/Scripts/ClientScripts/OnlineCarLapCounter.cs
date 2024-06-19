using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class OnlineCarLapCounter : MonoBehaviour
{
    public NetworkPlayer myPlayer;

    GameManager gameManager;
    TopDownCarController topDownCarController;

    public Text carPositionText;

    private Stopwatch timer;

    bool isRaceCompleted = false;

    bool isHideRoutineRunning = false;
    float hideUIDelayTime;

    public event Action<OnlineCarLapCounter> OnPassCheckpoint;

    public static bool ReremoveConrol = false;

    private void Awake()
    {
        //  gameManager = FindObjectOfType<GameManager>();
    }

    public bool isRaceComplete()
    {
        return isRaceCompleted;
    }

    // Початок корутини для відображення позиції гравця
    IEnumerator ShowPositionCO(float delayUntilHidePosition)
    {
        hideUIDelayTime += delayUntilHidePosition;

        carPositionText.text = myPlayer.carPosition.ToString();

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
         if(myPlayer.playerName != CrossScaneInfoHolder.GamerNickName)
        {
            return;
        }
        if (collider2D.CompareTag("CheckPoints"))
        {
            if (timer == null)
            {
                timer = new Stopwatch();
                timer.Start();
                Debug.Log("timerStarted");
            }
            if (isRaceCompleted)
                return;
            CheckPoint checkPoint = collider2D.GetComponent<CheckPoint>();

            if (myPlayer.passedCheckPointNumber + 1 == checkPoint.checkPointNumber)
            {
                myPlayer.passedCheckPointNumber = checkPoint.checkPointNumber;

                myPlayer.numberOfPassedCheckpoints++;

                myPlayer.timeAtLastPassedCheckPoint = (float)timer.Elapsed.TotalSeconds;

                OnPassCheckpoint?.Invoke(this);

                OnPlayerPassedChecpoint request = new OnPlayerPassedChecpoint { player = myPlayer };
                NetworkClient.Send(request);

                // timeAtLastPassedCheckPoint = Time.time;

                if (checkPoint.isFinishLine)
                {
                    myPlayer.passedCheckPointNumber = 0;
                    myPlayer.lapsCompleted++;

                   /* if (myPlayer.lapsCompleted >= GameRoom.NumberOfLaps)
                    {
                        isRaceCompleted = true;
                        gameManager.OnRaceEndForMe();
                    }*/
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
