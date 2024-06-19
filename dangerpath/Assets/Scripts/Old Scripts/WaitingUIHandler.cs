using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaitingUIHandler : MonoBehaviour
{
    public GameObject playerListTextPrefab;

    public Button startGameButton;

    PlayerListTextHandler[] playerListTextHandler;

   /* private void Awake()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            startGameButton.gameObject.SetActive(true);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
           // startGameButton.gameObject.SetActive(true);
        }
            // Ліпше викликати CreatePlayerListText() тут, так як може знадобитися оновлення списку гравців при старті
            CreatePlayerListText();
    }

    // Update is called once per frame
    void Update()
    {
        // Додайте вашу логіку оновлення, якщо вона потрібна
    }

    public void StartGameCountDown()
    {
       
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager is null!");
            return;
        }

        photonView.RPC("OnWaitingEndRPC", RpcTarget.All);
        Debug.Log("WaitingUIHandler: StartGameCountDown");
    }


    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CreatePlayerListText();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        CreatePlayerListText();
    }

    private void CreatePlayerListText()
    {
        VerticalLayoutGroup playerListLayoutGroup = GetComponentInChildren<VerticalLayoutGroup>();

        foreach (Transform child in playerListLayoutGroup.transform)
        {
            Destroy(child.gameObject);
        }

        playerListTextHandler = new PlayerListTextHandler[PhotonNetwork.PlayerList.Length];

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            GameObject playerListTextGameObject = Instantiate(playerListTextPrefab, playerListLayoutGroup.transform);

            playerListTextHandler[i] = playerListTextGameObject.GetComponent<PlayerListTextHandler>();

            int playerNumber = i + 1;
            playerListTextHandler[i].SetPlayerNameText("Player" + playerNumber.ToString());
        }
    }

    [PunRPC]
    private void OnWaitingEndRPC()
    {
        GameManager.Instance.OnWaitingEnd();
        gameObject.SetActive(false);
    }*/
}
