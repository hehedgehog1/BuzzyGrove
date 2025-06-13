using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private const int DaySegments = 3;

    public float dayTimer = 300f; // 5 minutes
    public string timeOfDay = "";
    public bool gameOver = false;
    public Button newDayButton;
    public Button leaveButton;
    public int flowerCount = 0;
    public int honeyCount = 0;
    public bool isFirstDay = true;
    public float dayLength;
    public float daySegmentLength;

    private UIManager uiManager;
    private int highScore;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
        dayLength = dayTimer;
        daySegmentLength = dayLength / DaySegments;

        CheckIfIsFirstDay();
        if (isFirstDay)
        {
            uiManager.StartTutorialSpeech();
        }
        else
        {
            uiManager.ReturnSpeech();
        }
    }       

    void Update()
    {
        UpdateDayLeft();

        if (!gameOver)
        {
            // Timer logic
            if (dayTimer > 0)
            {
                dayTimer -= Time.deltaTime; // Decrement timer by the time passed since last frame
            }
            else
            {
                dayTimer = 0;
                GameOver();
            }
        }
    }

    private void UpdateDayLeft()
    {
        var morningSegment = dayLength - daySegmentLength;
        var afternoonSegment = morningSegment - daySegmentLength;

        if (dayTimer > morningSegment)
        {
            timeOfDay = "Morning";
        }
        else if (dayTimer > afternoonSegment)
        {
            timeOfDay = "Afternoon";            
        }
        else if (gameOver)
        {
            timeOfDay = "Goodnight!";
        }
        else
        {
            timeOfDay = "Evening!";
        }

        uiManager.UpdateTimeOfDayText();
    }

    internal void UpdateHoneyCount(int honeyToAdd)
    {
        honeyCount += honeyToAdd;
        uiManager.UpdateHoneyText();
        Debug.Log("Honey produced! Total honey: " + honeyCount);
    }

    internal void UpdateFlowerCount(int flowerToAdd)
    {
        flowerCount += flowerToAdd;
    }

    private void GameOver()
    {
        gameOver = true;
        Debug.Log("Day has ended! HoneyCount = " + honeyCount);

        uiManager.EndOfDaySpeech(honeyCount, GetHighScore(), isFirstDay);

        SetHighScore();

        leaveButton.gameObject.SetActive(true);
        newDayButton.gameObject.SetActive(true);
    }

    void CheckIfIsFirstDay()
    {        
        int currentHighScore = GetHighScore();
        Debug.Log("The current high score is " + currentHighScore);

        if (currentHighScore == 0)
        {
            isFirstDay = true;
        }
        else
        {
            isFirstDay= false;
        }
        Debug.Log("isFirstDay = " + isFirstDay);
    }

    private void SetHighScore()
    {
        int currentHighScore = GetHighScore();

        if (honeyCount > currentHighScore)
        {
            // Update the high score
            PlayerPrefs.SetInt("HighScore", honeyCount);
            PlayerPrefs.Save();
            Debug.Log("New high score set: " + honeyCount);
        }
        else
        {
            Debug.Log("Score " + honeyCount + " did not exceed the high score: " + currentHighScore);
        }
    }

    public int GetHighScore()
    {
        return PlayerPrefs.GetInt("HighScore", 0);
    }

}
