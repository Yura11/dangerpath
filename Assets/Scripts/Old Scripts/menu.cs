using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
public class menu : MonoBehaviour
{
    public static bool Buttonstate=true;
    public static string previousSceneIndex;

    private void Start()
    {
       //  PhotonNetwork.Disconnect();
    }

    public void Play()
    {
        previousSceneIndex = "Game";
        SceneManager.LoadScene("Game");
    }
    public void Quit()
    {
        Application.Quit();
        Debug.Log("Player has quit");
    }
    public void PlayOnine()
    {
        previousSceneIndex = "GameOnline";
        SceneManager.LoadScene("Loading screen");
    }

}