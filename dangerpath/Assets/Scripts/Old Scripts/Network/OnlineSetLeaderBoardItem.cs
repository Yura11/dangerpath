using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineSetLeaderBoardItem : MonoBehaviour
{
    public GameObject leaderboardItemPrefab;

    SetLeaderBoardItemInfo[] setLeaderboardItemInfo; // Масив для зберігання інформації про пункти таблиці лідерів

    // Знаходимо компонент VerticalLayoutGroup у дочірньому об'єкті ії даного об'єкта
    VerticalLayoutGroup leaderboardLayoutGroup;
    void Awake()
    {
        leaderboardLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        // Знаходимо всі об'єкти CarLapCounter у сцені
        CarLapCounter[] carLapCounterArray = FindObjectsOfType<CarLapCounter>();

        // Ініціалізуємо масив, який буде зберігати інформацію про пункти таблиці лідерів

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

       // Оновлення списку лідерів на основі переданого списку CarLapCounter


       private void AddPlayerLeaderboardInfo()
       {
           // Створюємо новий масив з довжиною, більшою на один, ніж існуючий масив
           SetLeaderBoardItemInfo[] newSetLeaderboardItemInfo = new SetLeaderBoardItemInfo[setLeaderboardItemInfo.Length];

           // Копіюємо всі існуючі елементи в новий масив
           if (setLeaderboardItemInfo.Length != newSetLeaderboardItemInfo.Length)
           {
               for (int i = 0; i < setLeaderboardItemInfo.Length - 1; i++)
               {
                   newSetLeaderboardItemInfo[i] = setLeaderboardItemInfo[i];
               }
           }
           // Тепер змінна setLeaderboardItemInfo вказує на новий масив
           setLeaderboardItemInfo = newSetLeaderboardItemInfo;

           // Проходимось по усіх CarLapCounter і створюємо відповідні об'єкти таблиці лідерів
           for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
           {
               GameObject leaderboardInfoGameObject = Instantiate(leaderboardItemPrefab, leaderboardLayoutGroup.transform);

               // Додаємо компонент SetLeaderBoardItemInfo до створеного об'єкта
               setLeaderboardItemInfo[i] = leaderboardInfoGameObject.GetComponent<SetLeaderBoardItemInfo>();

               // Встановлюємо текст позиції для кожного пункта таблиці лідерів
               setLeaderboardItemInfo[i].SetPositionText($"{i + 1}.");
               foreach (Player player in PhotonNetwork.PlayerList)
               {
                   setLeaderboardItemInfo[i].SetDriverName(player.NickName);
               }
           }
       }*/
    }
}
