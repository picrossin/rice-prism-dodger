using System;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    [SerializeField] private string obstructionTag = "Obstruction";
    [SerializeField] private string levelTag = "Level";
    
    private GameObject[] _obstructions;

    private void Start()
    {
        if (!PlayerPrefs.HasKey(Obstruction.Movement.Horizontal.ToString()))
        {
            // Some editable presets for obstruction data
            CreateObstructionJSON(Obstruction.Movement.Horizontal, Color.green, 3f, 0f);
            CreateObstructionJSON(Obstruction.Movement.Vertical, Color.red, 3f, 0f);
            CreateObstructionJSON(Obstruction.Movement.Rotate, new Color(0.66f, 0f, 1f), 1.5f, 2f);
        }

        GameObject.FindGameObjectWithTag(levelTag).GetComponent<LevelGenerator>().GenerateLevel(6, 3);
        LoadObstructionData();
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
}