using UnityEngine;

public class TimeManager : MonoBehaviour
{
    [Tooltip("The average number of seconds it takes to clear a regular piece.")] [SerializeField]
    private float regularPieceGoalTime = 3.0f;

    [Tooltip("The average number of seconds it takes to clear a turn piece.")] [SerializeField]
    private float turnPieceGoalTime = 2.0f;

    [Tooltip("The calculated three-star time - the two-star threshold = two-star time.")] [SerializeField]
    private float twoStarThreshold = 4.0f;

    private float _timer;
    public float Timer => _timer;

    private int _currentLevelBestTime;
    public int CurrentLevelBestTime => _currentLevelBestTime;

    private float _threeStarGoal;
    private bool _counting;

    private void Update()
    {
        if (_counting)
        {
            _timer += Time.deltaTime;
        }
    }

    public void StartTimer(int regularPieceCount, int turnPieceCount, int level)
    {
        _timer = 0.0f;
        _counting = true;

        _currentLevelBestTime = PlayerPrefs.HasKey($"level{level}") ? PlayerPrefs.GetInt($"level{level}") : 0;

        _threeStarGoal = regularPieceCount * regularPieceGoalTime + turnPieceCount * turnPieceGoalTime;
    }

    public void StopAndLogTime(int level)
    {
        _counting = false;

        bool setHighScore = true;
        if (PlayerPrefs.HasKey($"level{level}"))
        {
            int currentBestTime = PlayerPrefs.GetInt($"level{level}");
            if ((int) _timer >= currentBestTime)
            {
                setHighScore = false;
            }
        }

        if (setHighScore)
        {
            PlayerPrefs.SetInt($"level{level}", (int) _timer);
            _currentLevelBestTime = (int) _timer;
        }
    }
}