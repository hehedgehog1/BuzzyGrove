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

    public int flowerCount = 0;
    public int honeyCount = 0;

    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();

        dayLength = dayTimer;
        daySegmentLength = dayLength / 3;

        uiManager.StartTutorialSpeech();
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
        newDayButton.gameObject.SetActive(true);
    }
}
