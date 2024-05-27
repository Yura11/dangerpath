using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class LeaderboardUIHandler : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;
    private OnlinePositionHandler onlinePositionHandler;
    private SetLeaderBoardItemInfo[] setLeaderboardItemInfo;
    public string[] WinnersList;

    void Start()
    {
        onlinePositionHandler = FindObjectOfType<OnlinePositionHandler>();

        // Initialize the array with the sorted player list
        if (CrossScaneInfoHolder.PlayerList != null && CrossScaneInfoHolder.PlayerList.Count > 0)
        {
            CreateAndFillLeaderboardItems();
        }
        else
        {
            Debug.LogWarning("Player list is null or empty.");
        }

        CreateLeaderBoardItems();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateLeaderBoardItems();
        }
    }

    public void CreateLeaderBoardItems()
    {
        Transform leaderboardLayoutGroup = transform.Find("Leaderboar Liast");

        // Check if the leaderboard layout group is found
        if (leaderboardLayoutGroup == null)
        {
            Debug.LogError("Leaderboard layout group not found.");
            return;
        }

        // Clear previous leaderboard items
        foreach (Transform child in leaderboardLayoutGroup)
        {
            Destroy(child.gameObject);
        }

        // Ensure sorted player list
        List<NetworkPlayer> sortedPlayerList = CrossScaneInfoHolder.PlayerList
            .OrderByDescending(player => player.passedCheckPointNumber)
            .ThenBy(player => player.timeAtLastPassedCheckPoint)
            .ToList();

        setLeaderboardItemInfo = new SetLeaderBoardItemInfo[sortedPlayerList.Count];

        for (int i = 0; i < sortedPlayerList.Count; i++)
        {
            GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup);

            setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderBoardItemInfo>();

            setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
            setLeaderboardItemInfo[i].SetDriverName(sortedPlayerList[i].playerName);
        }
    }

    void CreateAndFillLeaderboardItems()
    {
        // Sort the player list
        CrossScaneInfoHolder.PlayerList = CrossScaneInfoHolder.PlayerList
            .OrderByDescending(player => player.passedCheckPointNumber)
            .ThenBy(player => player.timeAtLastPassedCheckPoint)
            .ToList();

        // Initialize the array
        setLeaderboardItemInfo = new SetLeaderBoardItemInfo[CrossScaneInfoHolder.PlayerList.Count];

        // Fill the array
        for (int i = 0; i < CrossScaneInfoHolder.PlayerList.Count; i++)
        {
            setLeaderboardItemInfo[i] = new SetLeaderBoardItemInfo
            {
                playerName = CrossScaneInfoHolder.PlayerList[i].playerName,
                // Add other necessary properties to SetLeaderBoardItemInfo if needed
            };
        }

        Debug.Log("Leaderboard items created and filled.");
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
