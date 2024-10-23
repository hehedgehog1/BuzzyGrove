using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] seedPrefabs;
    public GameObject soilPatchPrefab;
    private int seedCount = 0;
    private int soilPatchCount = 0;
    private int spawnLimit = 50;
    private float spawnRangeX = 20;
    private float spawnRangeZ = 20;
    private float startDelay = 5;
    private float spawnInterval = 5f;

    // Start is called before the first frame update
    void Start()
    {
        if (seedCount < spawnLimit)
        {
            InvokeRepeating("SpawnSeed", startDelay, spawnInterval);
            seedCount++;
        }
        if (soilPatchCount < spawnLimit)
        {
            InvokeRepeating("SpawnSoilPatch", startDelay, spawnInterval);
            soilPatchCount++;
        }   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SpawnSeed()
    {
        //Only 1 seed prefab now, but hopefully more in future.
        int seedIndex = Random.Range(0, seedPrefabs.Length);
        //TODO: Update the spawn range for spawns
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, Random.Range(-spawnRangeZ, spawnRangeZ));

        Instantiate(seedPrefabs[seedIndex], spawnPos, seedPrefabs[seedIndex].transform.rotation);

    }

    void SpawnSoilPatch()
    {
        //TODO: Update how often soil patches get spawned
        //TODO: Dont't let spawns happen underneath cabin
        //TODO: Check soils cant spawn on top of eachother
        //TODO: Soils should spawn nearer eachother
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0.03f, Random.Range(-spawnRangeZ, spawnRangeZ));

        Instantiate(soilPatchPrefab, spawnPos, soilPatchPrefab.transform.rotation);
    }
}
