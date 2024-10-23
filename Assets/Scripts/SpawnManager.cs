using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] seedPrefabs;
    private float spawnRangeX = 20;
    private float spawnRangeZ = 20;
    private float startDelay = 5;
    private float spawnInterval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("SpawnSeed", startDelay, spawnInterval);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnSeed()
    {
        //Only 1 seed prefab now, but hopefully more in future.
        int seedIndex = Random.Range(0, seedPrefabs.Length);
        //TODO: Update the spawn range for seeds
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, Random.Range(-spawnRangeZ, spawnRangeZ));

        Instantiate(seedPrefabs[seedIndex], spawnPos, seedPrefabs[seedIndex].transform.rotation);

    }
}
