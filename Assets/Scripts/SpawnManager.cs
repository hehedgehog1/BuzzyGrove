using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject[] seedPrefabs;
    public GameObject grassPatchPrefab;
    private int seedCount = 0;
    private int grassPatchCount = 0;
    private int spawnLimit = 50;
    private float spawnRangeX = 50;
    private float spawnRangeZ = 50;
    private float startDelay = 5;
    private float spawnInterval = 5f;
    private float minSpawnDistance = 2.0f;
    private GameManager gameManager;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        if (seedCount < spawnLimit)
        {
            InvokeRepeating("SpawnSeed", startDelay, spawnInterval);
            seedCount++;
        }
        if (grassPatchCount < spawnLimit)
        {
            InvokeRepeating("SpawnGrassPatch", startDelay, spawnInterval);
            grassPatchCount++;
        }   
    }

    void SpawnSeed()
    {
        if (gameManager.gameOver)
        {
            CancelInvoke("SpawnSeed"); // Stop spawning if game is over
            return;
        }

        int seedIndex = Random.Range(0, seedPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0, Random.Range(-spawnRangeZ, spawnRangeZ));

        Instantiate(seedPrefabs[seedIndex], spawnPos, seedPrefabs[seedIndex].transform.rotation);

    }

    void SpawnGrassPatch()
    {
        if (gameManager.gameOver)
        {
            CancelInvoke("SpawnGrassPatch"); // Stop spawning if game is over
            return;
        }

        Vector3 spawnPos;

        // Try finding a valid spawn position
        bool validPositionFound = false;
        int attempts = 0;

        do
        {
            spawnPos = new Vector3(Random.Range(-spawnRangeX, spawnRangeX), 0.03f, Random.Range(-spawnRangeZ, spawnRangeZ));

            validPositionFound = IsValidSpawnPosition(spawnPos);
            attempts++;

            // Prevent infinite loop
            if (attempts >= 100) break;

        } while (!validPositionFound);

        if (validPositionFound)
        {
            Instantiate(grassPatchPrefab, spawnPos, grassPatchPrefab.transform.rotation);
        }
        else
        {
            Debug.LogWarning("Could not find a valid spawn position after multiple attempts.");
        }
    }

    private bool IsValidSpawnPosition(Vector3 position)
    {
        // Check for overlaps with other grass patches or objects
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
