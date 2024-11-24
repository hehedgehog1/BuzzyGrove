using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float dayTimer = 300f; //5 mins
    public string timeOfDay = "";
    public bool gameOver = false;
    private UIManager uiManager;

    public int seedCount = 0;
    public int flowerCount = 0;
    public int honeyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        uiManager = FindObjectOfType<UIManager>();
    }

    // Update is called once per frame
    void Update()
    {
        updateDayLeft();

        if (!gameOver)
        {
            // Timer logic
            if (dayTimer > 0)
            {
                dayTimer -= Time.deltaTime; // Decrement timer by the time passed since last frame
                //Debug.Log("current time: " + dayTimer);
            }
            else
            {
                dayTimer = 0;
                GameOver();
            }
        }
    }

    private void updateDayLeft()
    {
        if (dayTimer > 240f)
        {
            timeOfDay = "Morning";
            uiManager.UpdateTimeOfDayText();
        }
        else if (dayTimer > 120f)
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

    public void UpdateSeedCount(int seedToAdd)
    {
        seedCount += seedToAdd;
        uiManager.UpdateSeedText();
    }

    public void UpdateHoneyCount(int honeyToAdd)
    {
        honeyCount += honeyToAdd;
        uiManager.UpdateHoneyText();
    }

    public void UpdateFlowerCount(int flowerToAdd)
    {
        flowerCount += flowerToAdd;
    }

    private void GameOver()
    {
        gameOver = true;
        Debug.Log("Day has ended! HoneyCount = " + honeyCount);
    }
}
