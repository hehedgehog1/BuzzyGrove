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
    private float minSpawnDistance = 2.0f;

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
        Vector3 spawnPos;

        // Try finding a valid spawn position
        bool validPositionFound = false;
        int attempts = 0;

        do
        {
            spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0.03f, Random.Range(-spawnRangeZ, spawnRangeZ));

            validPositionFound = IsValidSpawnPosition(spawnPos);
            attempts++;

            // Debugging output
            Debug.Log($"Attempt {attempts}: Trying spawn position {spawnPos}, Valid: {validPositionFound}");

            // Prevent infinite loop
            if (attempts >= 100) break;

        } while (!validPositionFound);

        if (validPositionFound)
        {
            Instantiate(soilPatchPrefab, spawnPos, soilPatchPrefab.transform.rotation);
        }
        else
        {
            Debug.LogWarning("Could not find a valid spawn position after multiple attempts.");
        }
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Check for overlaps with other soil patches or objects
        Collider[] hitColliders = Physics.OverlapSphere(position, minSpawnDistance);

        foreach (Collider collider in hitColliders)
        {
            if (collider.CompareTag("Ground"))
            {
                continue; // ignore if collides with ground
            }
            // else pos is invalid
            return false;
        }

        // If no other colliders were found, the position is valid
        return true;
    }
}
