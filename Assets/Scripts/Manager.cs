using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

[RequireComponent(typeof(TimeManager))]
public class Manager : MonoBehaviour
{
    public static Manager Instance { set; get; }

    [Header("Player Setup")] 
    [SerializeField] private int lives;
    public int Lives => lives;
    
    [Header("Level Difficulty Setup")] 
    [SerializeField] private int regularPieceCount = 2;
    [SerializeField] private int turnPieceCount = 1;
    
    [Tooltip("Levels between regular piece count increases.")] [SerializeField] 
    private int regularPieceIncreaseFrequency = 1;
    
    [Tooltip("Levels between turn piece count increases.")] [SerializeField] 
    private int turnPieceIncreaseFrequency = 3;

    [Header("Tag Strings")]
    [SerializeField] private string obstructionTag = "Obstruction";
    [SerializeField] private string levelTag = "Level";
    [SerializeField] private string uiManagerTag = "UIManager";
    
    private TimeManager _timeManager;
    public TimeManager TimeManager => _timeManager;
    
    private int _level = 1;
    public int Level => _level;

    private GameObject[] _obstructions;
    private int _regularPieceIncreaseCounter = 0;
    private int _turnPieceIncreaseCounter = 0;

    private void Start()
    {
        // Make sure there is only one manager
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        _timeManager = GetComponent<TimeManager>();
        
        Random.InitState(222);

        if (!PlayerPrefs.HasKey(Obstruction.Movement.Horizontal.ToString()))
        {
            // Some editable presets for obstruction data
            CreateObstructionJSON(Obstruction.Movement.Horizontal, Color.green, 3f, 0f);
            CreateObstructionJSON(Obstruction.Movement.Vertical, Color.red, 3f, 0f);
            CreateObstructionJSON(Obstruction.Movement.Rotate, new Color(0.66f, 0f, 1f), 1.5f, 2f);
        }

        StartCoroutine(SetupLevel(regularPieceCount, turnPieceCount));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadNextLevelHelper());
    }

    private IEnumerator LoadNextLevelHelper()
    {
        UIManager uiManager = GameObject.FindGameObjectWithTag(uiManagerTag).GetComponent<UIManager>();
        
        bool gotBestTime = _timeManager.StopAndLogTime(_level);
        uiManager.SetTransitionText(_level, (int) _timeManager.Timer, gotBestTime);
        _level++;
        
        yield return StartCoroutine(uiManager.PlayAnimation("End"));
        yield return new WaitForSeconds(1.0f);
        
        MaybeIncreasePieceCount(regularPieceIncreaseFrequency, ref _regularPieceIncreaseCounter, ref regularPieceCount);
        MaybeIncreasePieceCount(turnPieceIncreaseFrequency, ref _turnPieceIncreaseCounter, ref turnPieceCount);
        
        StartCoroutine(SetupLevel(regularPieceCount, turnPieceCount, true));
    }

    public void ResetLevel()
    {
        lives--;
        if (lives <= 0)
        {
            _level = 1;
            lives = 3;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
        }
        else
        {
            GameObject.FindGameObjectWithTag(levelTag).GetComponent<LevelGenerator>().RespawnPlayer();
        }
        
        _timeManager.StartTimer(regularPieceCount, turnPieceCount, _level);
    }

    private IEnumerator SetupLevel(int newRegularPieceCount, int newTurnPieceCount, bool playAnimation=false)
    {
        GameObject.FindGameObjectWithTag(levelTag).GetComponent<LevelGenerator>()
            .GenerateLevel(newRegularPieceCount, newTurnPieceCount);
        LoadObstructionData();
        
        if (playAnimation)
        {
            yield return new WaitForSeconds(0.5f);
            yield return StartCoroutine(GameObject.FindGameObjectWithTag(uiManagerTag).GetComponent<UIManager>()
                .PlayAnimation("Start"));
        }
        
        _timeManager.StartTimer(regularPieceCount, turnPieceCount, _level);
        yield return null;
    }

    private void CreateObstructionJSON(Obstruction.Movement movementType, Color color,
        float speed, float rotationRadius)
    {
        // Format the data into a serializable object
        ObstructionData obstructionData = new ObstructionData();
        obstructionData.movementType = movementType.ToString();
        obstructionData.color = $"#{ColorUtility.ToHtmlStringRGB(color)}";
        obstructionData.speed = speed;
        obstructionData.rotationRadius = rotationRadius;

        // Convert the object to a JSON string and save it to the PlayerPrefs to load in later
        string json = JsonUtility.ToJson(obstructionData);
        PlayerPrefs.SetString(movementType.ToString(), json);
    }

    private void LoadObstructionData()
    {
        _obstructions = GameObject.FindGameObjectsWithTag(obstructionTag);

        // Build a dictionary of the different types of obstructions and their data
        Dictionary<string, ObstructionData> obstructionDataset = new Dictionary<string, ObstructionData>();
        string[] movementTypes = Enum.GetNames(typeof(Obstruction.Movement));
        foreach (string movementType in movementTypes)
        {
            if (PlayerPrefs.HasKey(movementType))
            {
                ObstructionData data = JsonUtility.FromJson<ObstructionData>(PlayerPrefs.GetString(movementType));
                obstructionDataset.Add(movementType, data);
            }
        }

        // For each obstruction in the scene, apply the data using the dictionary
        foreach (GameObject obstructionObject in _obstructions)
        {
            Obstruction obstruction = obstructionObject.GetComponent<Obstruction>();
            obstruction.SetData(obstructionDataset[obstruction.MovementType.ToString()]);
        }
    }

    private void MaybeIncreasePieceCount(int frequency, ref int counter, ref int pieceCount)
    {
        counter++;
        
        if (counter == frequency)
        {
            counter = 0;
            pieceCount++;
        }
    }
}