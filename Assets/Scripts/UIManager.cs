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
}
