using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSourceBehaviour : MonoBehaviour
{
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (playerController.waterCarried < 5)
        {
            //TODO: water audio sfx
            playerController.UpdateWaterCarried(1);
        }
    }
}
