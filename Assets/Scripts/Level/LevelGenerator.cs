﻿using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{
    [Header("Level Sections")] 
    [SerializeField] private GameObject startPiece;
    [SerializeField] private GameObject endPiece;
    [SerializeField] private GameObject leftTurnPiece;
    [SerializeField] private GameObject rightTurnPiece;
    [SerializeField] private GameObject[] otherLevelSections;

    private float _currentRotation;
    private Vector3 _currentLocation;
    private GameObject _startPieceInstance;

    public void GenerateLevel(int regularPieceCount, int turnPieceCount)
    {
        // Clear out old level, if any
        foreach (Transform section in transform)
        {
            Destroy(section.gameObject);
        }

        _currentRotation = 0f;
        _currentLocation = Vector3.zero;
        
        // Set up the start
        _startPieceInstance = Instantiate(startPiece, _currentLocation, CalculateRotation());
        SetParent(_startPieceInstance);

        StartSection startSection = _startPieceInstance.GetComponent<StartSection>();
        StartPieceExit startPieceExit = startSection.Exits[Random.Range(0, startSection.Exits.Length)];
        _currentRotation = startPieceExit.Rotation;
        _currentLocation = startPieceExit.transform.position;

        // Choose sections to generate
        List<GameObject> levelSections = new List<GameObject>();
        for (int i = 0; i < regularPieceCount; i++)
        {
            levelSections.Add(otherLevelSections[Random.Range(0, otherLevelSections.Length)]);
        }
        
        // Randomly distribute alternating left and right turns
        int divisions = levelSections.Count / turnPieceCount;
        for (int i = 0; i < turnPieceCount; i++)
        {
            levelSections.Insert(
                Random.Range(i * divisions, i * divisions + divisions - 1),
                i % 2 == 0 ? leftTurnPiece : rightTurnPiece);
        }

        // Generate level sections
        foreach (GameObject section in levelSections)
        {
            GameObject sectionInstance = Instantiate(section, _currentLocation, CalculateRotation());
            SetParent(sectionInstance);
            LevelSection levelSection = sectionInstance.GetComponent<LevelSection>();

            if (levelSection.IsLeftTurn)
            {
                _currentRotation -= 90f;
            }
            else if (levelSection.IsRightTurn)
            {
                _currentRotation += 90f;
            }
            
            _currentLocation = levelSection.Exit.transform.position;
        }
        
        // Generate level exit
        GameObject exitPieceInstance = Instantiate(endPiece, _currentLocation, CalculateRotation());
        SetParent(exitPieceInstance);
    }

    public void RespawnPlayer()
    {
        if (_startPieceInstance != null)
        {
            _startPieceInstance.GetComponent<StartSection>().PlayerSpawner.Spawn();
        }
    }

    private Quaternion CalculateRotation()
    {
        return Quaternion.Euler(0f, _currentRotation, 0f);
    }

    private void SetParent(GameObject objectToParent)
    {
        objectToParent.transform.parent = transform;
    }

    private void SetParent(Transform transformToParent)
    {
        transformToParent.parent = transform;
    }
}