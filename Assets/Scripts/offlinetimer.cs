using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

public class Offlinetimer : MonoBehaviour
{
    public Text offlineTimerText;
    private Stopwatch timer;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // ¬иводимо час у текст
        ShowTime();

        // –естартуЇмо таймер
        StartTimer();
    }

    private void StartTimer()
    {
        // ≤н≥ц≥ал≥зуЇмо таймер
        timer = new Stopwatch();
        timer.Start();
    }

    private void ShowTime()
    {
        // ¬иводимо час у текст
        if (timer != null)
        {
            float elapsedSeconds = (float)timer.Elapsed.TotalSeconds;
            offlineTimerText.text = elapsedSeconds.ToString("F2");
        }
    }
}
