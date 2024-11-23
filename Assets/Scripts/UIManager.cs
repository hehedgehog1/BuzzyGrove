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
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    public void UpdateSeedText()
    {
        seedText.text = "Seeds: " + playerController.seedCount;
    }

    public void UpdateTimeOfDayText()
    {
        timeOfDayText.text = playerController.timeOfDay;
    }

    public void UpdateHoneyText()
    {
        honeyText.text = "Honey: " + playerController.honeyCount;
    }
}
