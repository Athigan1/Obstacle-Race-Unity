using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public float timeRemaining;
    public bool timerIsRunning = false;
    [SerializeField] private TMP_Text timeText;

    private void Start()
    {
        int timerOn = PlayerPrefs.GetInt(MainMenu.TimerOn, 0);

        if (timerOn == 1)
        {
            // Starts the timer 
            timerIsRunning = true;
            timeRemaining = PlayerPrefs.GetInt(MainMenu.TimeRemaining, 0);
        }
    }

    void Update()
    {
        int timerOn = PlayerPrefs.GetInt(MainMenu.TimerOn, 0);

        if (timerOn == 0)
        {
            timerIsRunning = false;
        }

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                DisplayTime(timeRemaining);
                timeRemaining -= Time.deltaTime;
            }
            else
            {
                Debug.Log("Time has run out!");
                timeRemaining = 0;
                timerIsRunning = false;
            }
        }
    }

    void DisplayTime(float timeToDisplay)
    {
        float minutes = Mathf.FloorToInt(timeToDisplay / 60);
        float seconds = Mathf.FloorToInt(timeToDisplay % 60);

        timeText.text = string.Format("Full Energy in: {0:00}:{1:00}", minutes, seconds);
    }
}
