using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI timeOfDayText;
    public TextMeshProUGUI honeyText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI speechText;
    public TextMeshProUGUI highScoreText;
    public GameObject topBar;    
    public float typingSpeed = 0.05f;

    private GameManager gameManager;
    private PlayerController playerController;


    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    internal void UpdateSeedText()
    {
        seedText.text = "Seeds: " + playerController.seedCount;
    }

    internal void UpdateTimeOfDayText()
    {
        timeOfDayText.text = gameManager.timeOfDay;
    }

    internal void UpdateHoneyText()
    {
        honeyText.text = "Honey: " + gameManager.honeyCount;
    }

    internal void UpdateWaterText()
    {
        waterText.text = "Water: " + playerController.waterCarried;
    }

    internal void StartTutorialSpeech()
    {
        List<string> speechLines = new List<string>
        {
            "Bzz Buzz",
            "Hey you! I need your help buddy.",
            "There's no flowers around here.",
            "And I need flowers to make honey.",
            "Help a bee out.",
            "Ok, see that thing? That's a <color=#ff7e00>seed</color>.",
            "Go pick it up, then click on the <color=#613400>soil</color> to plant it.",
            "Sometimes birds come to eat the seeds.",
            "Tell them to <i>buzz off</i>.",
            "You need to water the soil too. Click on the <color=#1E90FF>well</color>",
            "Then <i>right click</i> on the soil.",
            "There's lots of <color=#71b61d>grassy</color> spots around here.",
            "Click on them to plant more flowers.",
            "You do that and I'll focus on making the sweet stuff.",
            "Let's see how much we can make in a day!"
        };

        ShowSpeech(speechLines);
    }

    internal void EndOfDaySpeech(int honeyCount, int highScore, bool isFirstDay)
    {
        List<string> endOfDayLines = new List<string>
        {
            "Bzz Bzz",
            "Hey, the day is over!",            
        };

        if (honeyCount == 0)
        {
            endOfDayLines.Add($"We made no honey!!!!!");
            endOfDayLines.Add("How terrible!!!!");
            endOfDayLines.Add("You should try harder tomorrow...");
        }
        else
        {
            if (honeyCount == 1)
            {
                endOfDayLines.Add($"We made {honeyCount} jar of honey together!");
            }
            else
            {
                endOfDayLines.Add($"We made {honeyCount} jars of honey together!");
            }

            if (isFirstDay)
            {
                endOfDayLines.Add("<size=70%><i>Oh boy, I'm gonna be one rich bee!</i></size>");
            }
            else if (honeyCount > highScore)
            {
                endOfDayLines.Add("That's our best day yet!");
            }
            else if (honeyCount < highScore)
            {
                endOfDayLines.Add($"<i>But it doesn't beat that day we made {highScore} jars...</i>");
            }
            else
            {
                endOfDayLines.Add("That's the same as our last record!");
            }

            endOfDayLines.Add("So, you want to do this again tomorrow?");
        }        

        ShowSpeech(endOfDayLines);
    }

    internal void ShowSpeech(List<string> lines)
    {
        if (!topBar.activeSelf)
        {
            topBar.SetActive(true);
        }
        StartCoroutine(DisplaySpeech(lines));
    }

    IEnumerator DisplaySpeech(List<string> lines)
    {
        foreach (string line in lines)
        {
            yield return StartCoroutine(TypeText(line));
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0));
        }
        topBar.SetActive(false); // Hide top bar when finished
    }

    IEnumerator TypeText(string line)
    {
        speechText.text = "";
        bool isTag = false;

        foreach (char c in line)
        {            
            if (c == '<') isTag = true; // process tagged words together
            if (isTag) speechText.text += c;
            else speechText.text += c; 

            if (c == '>') isTag = false; //end of tag.

            if (!isTag) yield return new WaitForSeconds(typingSpeed); //Only delay for visible characters.
        }
    }

    internal void ReturnSpeech()
    {
        List<string> speechLines = new List<string>
        {
            "Bzz Buzz",
            "Good morning!",
            "Let's make more honey today!"
        };

        ShowSpeech(speechLines);
    }
}
