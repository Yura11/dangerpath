using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;

public class connectToServer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
       // PhotonNetwork.ConnectUsingSettings();
    }

   // public override void OnConnectedToMaster()
    //{
     //   PhotonNetwork.JoinLobby();
    //}
    
    // Small typo fix: "OnJoinedLoby" should be "OnJoinedLobby"
   // public override void OnJoinedLobby()
   // {
    //    SceneManager.LoadScene("Lobby");
   // }
}
