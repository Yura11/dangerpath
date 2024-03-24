using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class CountDownUIHandler : MonoBehaviour
{
    [Header("Mixers")]
    public AudioMixer countDownMixer;

    [Header("AudioSources")]
    public AudioSource CountDown;


    public Text countDownText;

    private void Awake()
    {
        gameObject.SetActive(true);
        countDownText.text = " ";
    }

    // Start is called before the first frame update
    public void CountDownStart()
    {
        StartCoroutine(CountDownCO());
    }

    void Start()
    {
      
    }

    IEnumerator CountDownCO()
    {
        yield return new WaitForSeconds(0.3f);

        int counter = 3;
        CountDown.Play();
        while (true)
        {
            
            if (counter != 0)
                countDownText.text = counter.ToString();
            else
            {
                countDownText.text= "GO!";

                GameManager.Instance.OnRaceStart();
                yield return new WaitForSeconds(0.5f);

                break;
            }

            counter--;
            yield return new WaitForSeconds(1.0f);
        }

        yield return new WaitForSeconds(0.5f);

        gameObject.SetActive(false);
    }
}
