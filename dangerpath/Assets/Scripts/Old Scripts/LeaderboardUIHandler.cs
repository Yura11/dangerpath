using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{

    public GameObject leaderboardItemPrefab;

    OnlinePositionHandler onlinePositionHandler;
    SetLeaderBoardItemInfo[] setLeaderboardItemInfo;
    public string[] WinnersList;

    void Start()
    {
        onlinePositionHandler = FindObjectOfType<OnlinePositionHandler>();
        setLeaderboardItemInfo = new SetLeaderBoardItemInfo[CrossScaneInfoHolder.PlayerList.Count];
        CreateLeaderBoardItems();
    }

    // Start is called before the first frame update
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateLeaderBoardItems();
        }
    }

    public void CreateLeaderBoardItems()
    {
        VerticalLayoutGroup leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        // Check if the VerticalLayoutGroup component is found
        if (leaderboardLayoutGroup == null)
        {
            Debug.LogError("VerticalLayoutGroup component not found.");
            return;
        }

        // Clear previous leaderboard items
        foreach (Transform child in leaderboardLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        setLeaderboardItemInfo = new SetLeaderBoardItemInfo[carLapCounterArray.Length];

        for (int i = 0; i < carLapCounterArray.Length; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderBoardItemInfo>();

            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
            setLeaderboardItemInfo[i].SetDriverName(carLapCounterArray[i].gameObject.name);
        }
    }


    public void UpdateList(List<CarLapCounter> lapCounters)
    {
        WinnersList = new string[CrossScaneInfoHolder.PlayerList.Count];

        for (int i = 0; i < lapCounters.Count; i++)
        {
            setLeaderboardItemInfo[i].SetDriverName(lapCounters[i].gameObject.name);
            WinnersList[i] = lapCounters[i].gameObject.name.ToString();
            Debug.Log("Player: " + WinnersList[i]);
        }
    }

    public string GetWinners(int i)
    {
        return WinnersList[i];
    }
}