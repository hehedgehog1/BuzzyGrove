using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI timeOfDayText;
    public TextMeshProUGUI honeyText;
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void UpdateSeedText()
    {
        seedText.text = "Seeds: " + gameManager.seedCount;
    }

    public void UpdateTimeOfDayText()
    {
        timeOfDayText.text = gameManager.timeOfDay;
    }

    public void UpdateHoneyText()
    {
        honeyText.text = "Honey: " + gameManager.honeyCount;
    }
}
