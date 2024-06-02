using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

public class carcrash : MonoBehaviour
{
    int h, g;
    private Stopwatch timer;
    public static float seconds;
    public GameObject car;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("GameOver");
        }
    }
public void UWON(){
       
    }
    private void Start()
    {
        timer = new Stopwatch();
        timer.Start();

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Square" && this.gameObject == car)
        {
            if (g == 1 && h == 1)
            {
                
                seconds = (float)timer.Elapsed.TotalSeconds;
                SceneManager.LoadScene("GameWon");
            }
            g = 0;
            h = 0;
            timer.Restart();
            Debug.Log("Car collided with Square!");
        }
        if (collision.gameObject.name == "Square1" && this.gameObject == car)
        {
            Debug.Log("Car collided with Square!");
            g = 1;

        }
        if (collision.gameObject.name == "Square2" && this.gameObject == car)
        {
            Debug.Log("Car collided with Square!");
            if (g == 1) h = 1;
        }

    }
    }
