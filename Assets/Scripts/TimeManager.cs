using UnityEngine;

public class TimeManager : MonoBehaviour
{
    private float _timer = 0.0f;
    private bool _counting;

    private void Update()
    {
        if (_counting)
        {
            _timer += Time.deltaTime;
        }
    }

    public void StartTimer()
    {
        _timer = 0.0f;
        _counting = true;
    }

    public void StopAndLogTime(int level)
    {
        _counting = false;

        bool setHighScore = true;
        if (PlayerPrefs.HasKey($"level{level}"))
        {
            int currentBestTime = PlayerPrefs.GetInt($"level{level}");
            if ((int) _timer <= currentBestTime)
            {
                setHighScore = false;
            }
        }

        if (setHighScore)
        {
            PlayerPrefs.SetInt($"level{level}", (int)_timer);
        }
        
        Debug.Log($"Time: {(int)_timer}");
    }
}
