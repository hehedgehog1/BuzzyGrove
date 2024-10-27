using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI seedText;
    public TextMeshProUGUI honeyText;
    public TextMeshProUGUI flowerText;
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
        honeyText.text = "Honey: " + playerController.honeyCount;
        flowerText.text = "Flowers: " + playerController.flowerCount;
    }

}
