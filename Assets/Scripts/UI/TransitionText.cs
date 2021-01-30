using TMPro;
using UnityEngine;

public class TransitionText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI timeText;
    [SerializeField] private GameObject newHighScoreText;
    [SerializeField] private GameObject[] stars;

    public void SetText(int completedLevel, int time, bool highScore, int starCount)
    {
        levelText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        foreach (GameObject star in stars)
        {
            star.SetActive(false);
        }
        
        levelText.text = $"level {completedLevel} complete";
        timeText.text = $"{time}s";
        newHighScoreText.SetActive(highScore);
        for (int i = 0; i < starCount; i++)
        {
            stars[i].SetActive(true);
        }
    }
}
