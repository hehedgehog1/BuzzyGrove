using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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

    // Update is called once per frame
    void Update()
    {
        seedText.text = "Seeds: " + playerController.seedCount;
        timeOfDayText.text = playerController.timeOfDay;
        honeyText.text = "Honey: " + playerController.honeyCount;        
    }

}
