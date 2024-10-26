using System.Collections;
using UnityEngine;

public class PlantFlower : MonoBehaviour
{
    public GameObject[] flowerPrefabs;
    public Material seededMaterial;          
    public Material defaultMaterial;
    public GameObject birdPrefab;
    public float birdChance = 0.3f;
    public float seedGrowingTime = 60f;
    public float birdEatingTime = 60f;
    private bool seedPlanted = false;
    public HoneyProduction honeyProduction;
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
            birdInstance = Instantiate(birdPrefab, transform.position, Quaternion.identity);
            StartCoroutine(BirdWaitThenEat());
        }
        
        Debug.Log("A bird will not appear...");

        // Wait for the seeded state duration (1 minute)
        yield return new WaitForSeconds(seedGrowingTime);

        if (seedPlanted)
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
        yield return new WaitForSeconds(birdEatingTime/2);

        if (seedPlanted && birdInstance != null)
        {
            Debug.Log("A bird ate a seed!");
            seedPlanted = false;
            Destroy(birdInstance);

            // Reset material after seed is eaten
            if (soilRenderer != null)
            {
                soilRenderer.material = defaultMaterial; 
            }
        }
    }    
}
