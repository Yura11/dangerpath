using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbyListUImanager : MonoBehaviour
{

    public GameObject createLobbyMenu;


    public void OnClickBackHomeBTN()
    {
        SceneManager.LoadScene("Main");
    }

    public void OnClickCreateLobbyBTN() 
    {
        gameObject.SetActive(false);
        createLobbyMenu.SetActive(true);
    }

   
}
