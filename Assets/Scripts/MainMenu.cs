using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeDuration;
    [SerializeField] private AndroidNotificationHandler androidNotificationHandler;
    [SerializeField] private Button playButton;

    private int energy;
    private const string initializeTimer = "InitTimer";
    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";
    public const string TimeRemaining = "TimeRemaining";
    public const string TimerOn = "TimerOn";

    private void Start()
    {
        OnApplicationFocus(true);
    }
    private void OnApplicationFocus(bool hasFocus)
    {
        if(!hasFocus) { return; }
        CancelInvoke();
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = $"High Score {highScore}";

        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if (energy == 0)
        {
            if (PlayerPrefs.GetInt(initializeTimer) == 1)
            {
                DateTime energyReadyTime = DateTime.Now.AddMinutes(energyRechargeDuration);
                PlayerPrefs.SetString(EnergyReadyKey, energyReadyTime.ToString());
                androidNotificationHandler.ScheduleNotification(energyReadyTime);
                PlayerPrefs.SetInt(TimerOn, 1);
                PlayerPrefs.SetInt(initializeTimer, 0);
            }
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);

            if (energyReadyString == string.Empty) { return;  }

            DateTime energyReady = DateTime.Parse(energyReadyString);

            if (DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
                PlayerPrefs.SetInt(TimerOn, 0);
            }
            else
            {
                playButton.interactable = false;
                int secondsRemaining = (energyReady - DateTime.Now).Seconds;
                Invoke(nameof(EnergyRecharged), secondsRemaining + 1);
                PlayerPrefs.SetInt(TimeRemaining, secondsRemaining);
            }
        }
        energyText.text = $"PLAY ({energy})";
    }

    private void EnergyRecharged()
    {
        playButton.interactable = true;
        PlayerPrefs.SetInt(TimerOn, 0);
        energy = maxEnergy;
        PlayerPrefs.SetInt(EnergyKey, energy);
        energyText.text = $"PLAY ({energy})";
    }

    public void Play()
    {
        if (energy < 1) { return; }
        energy--;
        PlayerPrefs.SetInt(EnergyKey, energy);

        SceneManager.LoadScene(1);
        if(energy == 0) 
        {
            PlayerPrefs.SetInt(initializeTimer, 1);
        }
    }
}
