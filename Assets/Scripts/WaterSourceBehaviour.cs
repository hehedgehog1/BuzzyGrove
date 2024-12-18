using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSourceBehaviour : MonoBehaviour
{
    public float interactionRadius;
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
        Debug.Log("Water clicked");
        if (IsPlayerNearby() && playerController.waterCarried < 5)
        {
            //TODO: water audio sfx
            Debug.Log("Water updated by 1");
            playerController.UpdateWaterCarried(1);
        }
    }

    private bool IsPlayerNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        Debug.Log("Player too far away to click");
        return false;
    }
}
