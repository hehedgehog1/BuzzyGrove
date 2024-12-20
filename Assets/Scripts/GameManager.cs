using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public float dayTimer = 300f; //5 mins
    private float dayLength;
    private float daySegmentLength;
    public string timeOfDay = "";
    public bool gameOver = false;
    private UIManager uiManager;    
    public Button newDayButton;
    public Button leaveButton;
    private int highScore;
    public bool isFirstDay = true;

    public int flowerCount = 0;
    public int honeyCount = 0;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        dayLength = dayTimer;
        daySegmentLength = dayLength / 3;

        IsFirstDay();

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
            uiManager.UpdateTimeOfDayText();
        }
        else if (dayTimer > afternoonSegment)
        {
            timeOfDay = "Afternoon";
            uiManager.UpdateTimeOfDayText();
        }
        else if (gameOver)
        {
            timeOfDay = "Goodnight!";
            uiManager.UpdateTimeOfDayText();
        }
        else
        {
            timeOfDay = "Evening!";
            uiManager.UpdateTimeOfDayText();
        }
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

    void IsFirstDay()
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
        int currentHighScore = PlayerPrefs.GetInt("HighScore", 0);

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
