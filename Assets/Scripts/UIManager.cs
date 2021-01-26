using System;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    [SerializeField] private TextMeshProUGUI bestTime;
    [SerializeField] private GameObject[] stars;
    [SerializeField] private TextMeshProUGUI level;

    private bool _starsInitialized;
    
    private void Update()
    {
        UpdateTimer();
        UpdateBestTime();
        UpdateStars();
        UpdateLevel();
    }

    private void OnEnable()
    {
        TimeManager.OnStarted += ResetStars;
    }

    private void OnDisable()
    {
        TimeManager.OnStarted -= ResetStars;
    }

    private void UpdateTimer()
    {
        timer.text = $"time: {FormatIntTime((int) Manager.Instance.TimeManager.Timer)}";
    }

    private void UpdateBestTime()
    {
        bestTime.text = $"best: {FormatIntTime(Manager.Instance.TimeManager.CurrentLevelBestTime, true)}";
    }

    private void UpdateLevel()
    {
        level.text = $"level {Manager.Instance.Level}";
    }

    private void UpdateStars()
    {
        TimeManager timeManager = Manager.Instance.TimeManager;

        _starsInitialized = false;
        
        int time = (int) timeManager.Timer;
        if (time > timeManager.ThreeStarGoal)
        {
            stars[stars.Length - 1].SetActive(false);
        }

        if (time > timeManager.ThreeStarGoal + timeManager.TwoStarThreshold)
        {
            stars[stars.Length - 2].SetActive(false);
        }
    }

    private void ResetStars()
    {
        foreach (GameObject star in stars)
        {
            star.SetActive(true);
        }
    }

    private string FormatIntTime(int seconds, bool checkForInvalidScore = false)
    {
        string timeText = "0";
        if (seconds < 10)
        {
            timeText += seconds.ToString();
        }
        else
        {
            timeText = seconds.ToString();
        }

        if (checkForInvalidScore && seconds == 0)
        {
            timeText = "—";
        }

        return timeText;
    }
}