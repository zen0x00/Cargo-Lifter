using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class Timer 
{
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] private uiManager ui;
    public float timeInSec = 30f;
    bool isTimerRunning = true;

    public void Play()
    {
        if (!isTimerRunning) return;

        if (timeInSec < 0)
        {
            timeInSec = 0;
            isTimerRunning = false;
            OnTimerFinished();
        }

        timeInSec -= Time.deltaTime;
        int minutes = (int)timeInSec / 60;
        int seconds = (int)timeInSec % 60;
        timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);

    }

    public void Stop()
    {
        isTimerRunning = false;
    }

    private void OnTimerFinished()
    {
        ui.ShowGameOver();

    }
}
