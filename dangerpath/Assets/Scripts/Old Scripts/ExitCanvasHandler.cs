
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ExitCanvasHandler : MonoBehaviour
{
    public GameObject exitCanvas;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            exitCanvas.SetActive(true);
        }
    }

    public void StayButton()
    {
        exitCanvas.SetActive(false);
    }

    public void Menubutton()
    {
       // if (PhotonNetwork.IsConnected)
       // {
            // PhotonNetwork.Disconnect();
           // PhotonNetwork.LeaveLobby();
            SceneManager.LoadScene("SampleScene");
       // }
        //else
        {
            Debug.LogError("Photon client is not connected.");
            SceneManager.LoadScene("SampleScene");
        }
       
   }

}
