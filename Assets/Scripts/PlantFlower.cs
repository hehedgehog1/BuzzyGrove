using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantFlower : MonoBehaviour
{
    public GameObject[] flowerPrefabs;
    private bool flowerPlanted = false;
    public HoneyProduction honeyProduction;

    // Start is called before the first frame update
    void Start()
    {
        honeyProduction = FindObjectOfType<HoneyProduction>();

        if (honeyProduction == null)
        {
            Debug.LogError("No HoneyProduction script found.");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !flowerPlanted)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null && playerController.seedCount > 0)
            {
                playerController.seedCount--;
                playerController.flowerCount++;

                flowerPlanted = true;

                Vector3 soilPosition = transform.position;

                //There is only one flower prefab atm, but incase of multiple in future
                int flowerIndex = Random.Range(0, flowerPrefabs.Length);

                Instantiate(flowerPrefabs[flowerIndex], soilPosition, Quaternion.identity);

                Debug.Log("A flower has been planted!");

                honeyProduction.StartMakingHoney();
            }            
        }
    }
}
