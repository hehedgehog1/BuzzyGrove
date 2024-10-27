using System.Collections;
using UnityEngine;

public class PlantFlower : MonoBehaviour
{
    public GameObject[] flowerPrefabs;
    public Material seededMaterial;          
    public Material defaultMaterial;
    public GameObject birdPrefab;
    private float birdChance = 0.5f;
    private bool birdEating = false;
    private float seedGrowingTime = 60f;
    private float birdEatingTime = 4f;
    private bool seedPlanted = false;
    private HoneyProduction honeyProduction;
    private PlayerController playerController;    
    private GameObject birdInstance;
    private Renderer soilRenderer;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();

        honeyProduction = FindObjectOfType<HoneyProduction>();

        if (honeyProduction == null)
        {
            Debug.LogError("No HoneyProduction script found.");
        }

        // Get the Renderer component to change materials
        soilRenderer = GetComponent<Renderer>();
        
        if (soilRenderer != null)
        {
            soilRenderer.material = defaultMaterial;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        // if player collides w unseeded soil
        if (other.CompareTag("Player") && !seedPlanted)
        {
            PlayerController playerController = other.GetComponent<PlayerController>();

            if (playerController != null && playerController.seedCount > 0)
            {
                Debug.Log("A seed has been planted!");
                playerController.seedCount--;
                
                if (soilRenderer != null)
                {
                    soilRenderer.material = seededMaterial; // Change to seeded material
                }

                PlantSeed();
            }            
        }
        // if player collides w seeded soil and theres a bird on it
        else if (other.CompareTag("Player") && seedPlanted && birdInstance != null)
        {
            Debug.Log("Player scared the bird away!");
            birdEating = false;
            StartCoroutine(BirdFliesAway());
        }
    }

    private IEnumerator BirdFliesAway()
    {
        Vector3 startPosition = birdInstance.transform.position;

        Vector3 offScreenPosition = new Vector3(60f, 10f, 10f);

        float duration = 1.0f; // Duration of the movement
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            // Smoothly moves between the start and offscreen position
            if (birdInstance != null) // Check if the birdInstance is still valid
            {
                birdInstance.transform.position = Vector3.Lerp(startPosition, offScreenPosition, t);
            }
            
            // Wait for next frame
            yield return null;
        }

        // Delete bird
        if (birdInstance != null)
        {
            Destroy(birdInstance);
        }
    }

    private void PlantSeed()
    {
        seedPlanted = true;
        StartCoroutine(SeededState());
    }

    private IEnumerator SeededState()
    {
        Debug.Log("Flowers are growing...");

        //Randomly decide if bird appears
        if (Random.value < birdChance)
        {
            Debug.Log("A bird will appear...");
            yield return new WaitForSeconds(seedGrowingTime/4);
            // Spawn the bird and start its behavior coroutine

            Vector3 birdPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);

            birdInstance = Instantiate(birdPrefab, birdPosition, Quaternion.identity);
            StartCoroutine(BirdWaitThenEat());
        }
        
        Debug.Log("A bird will not appear...");

        // Wait for the seeded state duration (1 minute)
        yield return new WaitForSeconds(seedGrowingTime);

        if (seedPlanted && !birdEating)
        {
            Debug.Log("The seed survived!");

            if (soilRenderer != null)
            {
                soilRenderer.material = defaultMaterial;
            }
            FloweredState();
        }
    }

    private void FloweredState()
    {    
        playerController.flowerCount++;

        Debug.Log("A flower has grown! Flower count: " + playerController.flowerCount);

        Vector3 soilPosition = transform.position;

        // There is only one flower prefab atm, but incase of multiple in future
        int flowerIndex = Random.Range(0, flowerPrefabs.Length);

        Instantiate(flowerPrefabs[flowerIndex], soilPosition, Quaternion.identity);

        honeyProduction.StartMakingHoney();
    }

    private IEnumerator BirdWaitThenEat()
    {
        Debug.Log("A bird has appeared!");
        birdEating = true;
        yield return new WaitForSeconds(birdEatingTime);

        if (seedPlanted && birdInstance != null)
        {
            Debug.Log("A bird ate a seed!");
            seedPlanted = false;
            Destroy(birdInstance);
            birdEating = false;

            // Reset material after seed is eaten
            if (soilRenderer != null)
            {
                soilRenderer.material = defaultMaterial; 
            }
        }
    }    
}
