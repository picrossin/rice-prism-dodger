using TMPro;
using UnityEngine;

public class TransitionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject newHighScoreText;

    public void SetText(int completedLevel, int time, bool highScore)
    {
        levelText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        
        levelText.text = $"level {completedLevel} complete";
        timeText.text = $"{time}s";
        newHighScoreText.SetActive(highScore);
    }
}
