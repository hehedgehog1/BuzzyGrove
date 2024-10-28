using System.Collections;
using UnityEngine;

public class PlantFlower : MonoBehaviour
{
    [Header("Game Objects & Materials")]
    public GameObject[] flowerPrefabs;
    public Material seededMaterial;
    public Material defaultMaterial;
    public GameObject birdPrefab;

    [Header("Audio")]
    public AudioClip plantSound;
    public AudioClip birdCawSound;

    [Header("Timing Settings")]
    private float birdChance = 0.5f;
    private float seedGrowingTime = 60f;
    private float birdEatingTime = 4f;

    private bool seedPlanted = false;
    private bool birdEating = false;

    private HoneyProduction honeyProduction;
    private PlayerController playerController;
    private GameObject birdInstance;
    private Renderer soilRenderer;
    private AudioSource plantAudio;

    // Start is called before the first frame update
    void Start()
    {
        plantAudio = GetComponent<AudioSource>();
        playerController = FindObjectOfType<PlayerController>();
        honeyProduction = FindObjectOfType<HoneyProduction>();
        soilRenderer = GetComponent<Renderer>();

        if (honeyProduction == null)
        {
            Debug.LogError("No HoneyProduction script found.");
        }

        SetSoilMaterial(defaultMaterial);
    }    

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // if player touches unseeded soil:
        if (!seedPlanted)
        {
            TryPlantSeed(other);
        }
        else if (birdInstance != null)
        {
            ScareBirdAway();
        }
    }

    private void TryPlantSeed(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>();

        if (playerController == null || playerController.seedCount <= 0) return;

        playerController.seedCount--;
        PlaySound(plantSound);
        SetSoilMaterial(seededMaterial);  
        seedPlanted = true;
        Debug.Log("A seed has been planted!");
        StartCoroutine(SeededState());
    }

    private void ScareBirdAway()
    {
        PlaySound(birdCawSound);
        birdEating = false;
        StopCoroutine(BirdWaitThenEat());

        Debug.Log("Player scared the bird away!");
        StartCoroutine(BirdFliesAway());
    }

    private IEnumerator SeededState()
    {
        //Randomly decide if bird appears
        if (Random.value < birdChance)
        {
            Debug.Log("A bird will appear...");
            yield return new WaitForSeconds(seedGrowingTime / 4);
            SpawnBird();            
        }
        else
        {
            Debug.Log("A bird will not appear...");
        }        

        // Wait for seed to grow
        yield return new WaitForSeconds(seedGrowingTime);

        if (seedPlanted && !birdEating)
        {
            FloweredState();
        }            
    }

    private void SpawnBird()
    {
        Vector3 birdPosition = new Vector3(transform.position.x, 0.5f, transform.position.z);
        birdInstance = Instantiate(birdPrefab, birdPosition, Quaternion.identity);
        StartCoroutine(BirdWaitThenEat());
    }

    private void FloweredState()
    {
        Debug.Log("The seed survived!");
        SetSoilMaterial(defaultMaterial);

        playerController.flowerCount++;
        Debug.Log("A flower has grown! Flower count: " + playerController.flowerCount);

        // There is only one flower prefab atm, but incase of multiple in future
        int flowerIndex = Random.Range(0, flowerPrefabs.Length);
        Instantiate(flowerPrefabs[flowerIndex], transform.position, Quaternion.identity);

        honeyProduction.StartMakingHoney();
    }

    private IEnumerator BirdWaitThenEat()
    {        
        birdEating = true;
        Debug.Log("A bird has appeared!");

        yield return new WaitForSeconds(birdEatingTime);

        if (seedPlanted && birdInstance != null && birdEating)
        {
            Debug.Log("A bird ate a seed!");
            seedPlanted = false;
            Destroy(birdInstance);
            birdEating = false;
            SetSoilMaterial(defaultMaterial);            
        }
    }

    private IEnumerator BirdFliesAway()
    {
        Vector3 startPosition = birdInstance.transform.position;
        Vector3 offScreenPosition = new Vector3(60f, 10f, 10f);
        float duration = 1.0f; // Duration of bird flight
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
           
    private void SetSoilMaterial(Material material)
    {
        if (soilRenderer != null)
        {
            soilRenderer.material = material;
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (plantAudio != null && clip != null)
        {
            plantAudio.PlayOneShot(clip, 1.0f);
        }
    }
}
