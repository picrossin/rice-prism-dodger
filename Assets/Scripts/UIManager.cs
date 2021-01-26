using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timer;
    public TextMeshProUGUI Timer => timer;
    
    [SerializeField] private TextMeshProUGUI bestTime;
    public TextMeshProUGUI BestTime => bestTime;
    
    private void Update()
    {
        UpdateTimer();
        UpdateBestTime();
    }

    private void UpdateTimer()
    {
        timer.text = $"time: {FormatIntTime((int) Manager.Instance.TimeManager.Timer)}";
    }

    private void UpdateBestTime()
    {
        bestTime.text = $"best: {FormatIntTime(Manager.Instance.TimeManager.CurrentLevelBestTime, true)}";
    }

    private string FormatIntTime(int seconds, bool checkForInvalidScore=false)
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