using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSetLeaderBoardItem : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderBoardItemInfo[] setLeaderboardItemInfo; // ����� ��� ��������� ���������� ��� ������ ������� �����

    // ��������� ��������� VerticalLayoutGroup � ���������� ��'��� �� ������ ��'����
    VerticalLayoutGroup leaderboardLayoutGroup;
    void Awake()
    {
        leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        // ��������� �� ��'���� CarLapCounter � ����
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        // ���������� �����, ���� ���� �������� ���������� ��� ������ ������� �����

        /*   setLeaderboardItemInfo = new SetLeaderBoardItemInfo[PhotonNetwork.PlayerList.Length];

           AddPlayerLeaderboardInfo();
       }

       public override void OnPlayerEnteredRoom(Player newPlayer)
       {
           AddPlayerLeaderboardInfo();
       }

       // Start is called before the first frame update
       void Start()
       {

       }

       // ��������� ������ ����� �� ����� ���������� ������ CarLapCounter


       private void AddPlayerLeaderboardInfo()
       {
           // ��������� ����� ����� � ��������, ������ �� ����, �� �������� �����
           SetLeaderBoardItemInfo[] newSetLeaderboardItemInfo = new SetLeaderBoardItemInfo[setLeaderboardItemInfo.Length];

           // ������� �� ������� �������� � ����� �����
           if (setLeaderboardItemInfo.Length != newSetLeaderboardItemInfo.Length)
           {
               for (int i = 0; i < setLeaderboardItemInfo.Length - 1; i++)
               {
                   newSetLeaderboardItemInfo[i] = setLeaderboardItemInfo[i];
               }
           }
           // ����� ����� setLeaderboardItemInfo ����� �� ����� �����
           setLeaderboardItemInfo = newSetLeaderboardItemInfo;

           // ����������� �� ��� CarLapCounter � ��������� ������� ��'���� ������� �����
           for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
           {
               GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

               // ������ ��������� SetLeaderBoardItemInfo �� ���������� ��'����
               setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderBoardItemInfo>();

               // ������������ ����� ������� ��� ������� ������ ������� �����
               setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
               foreach (Player player in PhotonNetwork.PlayerList)
               {
                   setLeaderboardItemInfo[i].SetDriverName(player.NickName);
               }
           }
       }*/
    }
}
