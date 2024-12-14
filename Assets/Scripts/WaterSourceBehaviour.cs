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
        if (IsPlayerNearby() && playerController.waterCarried < 5)
        {
            //TODO: water audio sfx
            playerController.UpdateWaterCarried(1);
        }
    }

    private bool IsPlayerNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, playerController.interactionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        return false;
    }
}
