using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;

/*ublic class LobbyList : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    public GameObject lobbyEntryPrefab; // Префаб для створення елементів списку лоббі
    public Transform lobbyListContent; // Контейнер для відображення списку лоббі
    public Button refreshButton; // Кнопка для оновлення списку лоббі

    private void Start()
    {
        // Встановлення обробника подій для кнопки оновлення
        refreshButton.onClick.AddListener(RefreshLobbyList);
        // Оновлення списку лоббі при старті
        RefreshLobbyList();
    }

    // Оновлення списку лоббі
    public void RefreshLobbyList()
    {
        // Очистка списку лоббі перед оновленням
        foreach (Transform child in lobbyListContent)
        {
            Destroy(child.gameObject);
        }

        // Отримання списку доступних кімнат
      /*  foreach (KeyValuePair<string, Room> room in networkManager.rooms)
        {
            // Створення нового елементу списку лоббі
            GameObject lobbyEntry = Instantiate(lobbyEntryPrefab, lobbyListContent);

            // Отримання компонентів елементу UI для відображення інформації про кімнату
            Text roomNameText = lobbyEntry.GetComponentInChildren<Text>();
            Button joinButton = lobbyEntry.GetComponentInChildren<Button>();

            // Встановлення тексту для елементу списку лоббі
            roomNameText.text = room.Value.Name;

            // Встановлення обробника подій для кнопки приєднання до кімнати
            joinButton.onClick.AddListener(() => JoinRoom(room.Key));
        }
    }

    // Метод для приєднання до вибраної кімнати
    public void JoinRoom(string roomName)
    {
        // Приєднання до вибраної кімнати
        //networkManager.JoinRoom(roomName);
    }
}*/
