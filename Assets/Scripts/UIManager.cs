using System.Collections;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI timeOfDayText;
    public TextMeshProUGUI honeyText;
    public TextMeshProUGUI waterText;
    public TextMeshProUGUI speechText;
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
        string[] tutorialLines = {
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

        ShowSpeech(tutorialLines);
    }

    internal void ShowSpeech(string[] lines)
    {
        if (!topBar.activeSelf)
        {
            topBar.SetActive(true);
        }
        StartCoroutine(DisplaySpeech(lines));
    }

    IEnumerator DisplaySpeech(string[] lines)
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

}
