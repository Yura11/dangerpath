using UnityEngine;
using UnityEngine.UI;
using Mirror;
using System.Collections.Generic;

/*ublic class LobbyList : MonoBehaviour
{
    [SerializeField] NetworkManager networkManager;
    public GameObject lobbyEntryPrefab; // ������ ��� ��������� �������� ������ ����
    public Transform lobbyListContent; // ��������� ��� ����������� ������ ����
    public Button refreshButton; // ������ ��� ��������� ������ ����

    private void Start()
    {
        // ������������ ��������� ���� ��� ������ ���������
        refreshButton.onClick.AddListener(RefreshLobbyList);
        // ��������� ������ ���� ��� �����
        RefreshLobbyList();
    }

    // ��������� ������ ����
    public void RefreshLobbyList()
    {
        // ������� ������ ���� ����� ����������
        foreach (Transform child in lobbyListContent)
        {
            Destroy(child.gameObject);
        }

        // ��������� ������ ��������� �����
      /*  foreach (KeyValuePair<string, Room> room in networkManager.rooms)
        {
            // ��������� ������ �������� ������ ����
            GameObject lobbyEntry = Instantiate(lobbyEntryPrefab, lobbyListContent);

            // ��������� ���������� �������� UI ��� ����������� ���������� ��� ������
            Text roomNameText = lobbyEntry.GetComponentInChildren<Text>();
            Button joinButton = lobbyEntry.GetComponentInChildren<Button>();

            // ������������ ������ ��� �������� ������ ����
            roomNameText.text = room.Value.Name;

            // ������������ ��������� ���� ��� ������ ��������� �� ������
            joinButton.onClick.AddListener(() => JoinRoom(room.Key));
        }
    }

    // ����� ��� ��������� �� ������� ������
    public void JoinRoom(string roomName)
    {
        // ��������� �� ������� ������
        //networkManager.JoinRoom(roomName);
    }
}*/
