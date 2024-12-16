using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBehaviour : MonoBehaviour
{
    public GameObject soilPrefab;
    private PlayerController playerController;

    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    void OnMouseDown()
    {
        if (IsPlayerNearby())
        {
            Vector3 spawnPos = new Vector3(transform.position.x, -1.3f, transform.position.z);
            Instantiate(soilPrefab, spawnPos, soilPrefab.transform.rotation);
            Destroy(gameObject);
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
