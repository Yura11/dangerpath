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
        // �������� ��� � �����
        ShowTime();

        // ���������� ������
        StartTimer();
    }

    private void StartTimer()
    {
        // ���������� ������
        timer = new Stopwatch();
        timer.Start();
    }

    private void ShowTime()
    {
        // �������� ��� � �����
        if (timer != null)
        {
            float elapsedSeconds = (float)timer.Elapsed.TotalSeconds;
            offlineTimerText.text = elapsedSeconds.ToString("F2");
        }
    }
}
