using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerListItem : MonoBehaviour
{/*
    [SerializeField] TMP_Text text;
    Player player;
    [SerializeField] GameObject PlayerListItemPrefab;
    [SerializeField] Transform playerListContent;

    public void SetUp(Player _player)
    {
        player = _player;
        text.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Instantiate(PlayerListItemPrefab, playerListContent).GetComponent<PlayerListItem>().SetUp(newPlayer);
        PhotonNetwork.NickName = PhotonNetwork.LocalPlayer.ActorNumber.ToString();
        Debug.Log(newPlayer.NickName);
    }

public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }*/
}