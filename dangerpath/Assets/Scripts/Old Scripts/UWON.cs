using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using TMPro;



public class UWON : MonoBehaviour
{
    public Text timeText;
    public TextMeshProUGUI resultText;


    public void Start()
    {
        resultText.text = "“в≥й час: " + carcrash.seconds.ToString("0.00");
        gameObject.SetActive(true);
        
    }
    public void Restartbutton()
    {
        SceneManager.LoadScene("Game");
    }
    public void Menubutton()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
