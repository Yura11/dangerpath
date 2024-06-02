using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class GAMEOVER : MonoBehaviour

{

    public AudioSource soundEffect;
    public void Setup()
    {
        gameObject.SetActive(true);
        soundEffect.Play();
    }
    //public void Restartbutton()
   // {
    //    string previousScene = menu.previousSceneIndex;
    //    SceneManager.LoadScene(previousScene);
  //  }
   // public void Menubutton()
    //{
    //    SceneManager.LoadScene("SampleScene");
     //   PhotonNetwork.Disconnect();
  //  }
}
