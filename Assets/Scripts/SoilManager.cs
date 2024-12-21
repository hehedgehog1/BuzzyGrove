using System.Collections;
using UnityEngine;

public class SoilManager : MonoBehaviour
{
    [Header("Game Objects & Materials")]
    public GameObject[] saplingPrefabs;
    public GameObject[] flowerPrefabs;
    public GameObject seedPrefab;
    public Material soilPatchMaterial;
    public Material wetSoilPatchMaterial;
    public GameObject birdPrefab;

    [Header("Audio")]
    public AudioClip plantSound;

    [Header("Timing Settings")]
    private float birdChance = 0.6f;
    private float stage1_SeededGrowingTime = 30f;
    private float stage2_SaplingGrowingTime = 30f;
    private float birdSpawnTime = 3f;

    public bool seedPlanted = false;
    public bool isSapling = false;
    public bool isFlower = false;
    public bool isWatered = false;
    public bool isBirdEating = false;

    private Renderer soilRenderer;
    private AudioSource plantAudio;
    private GameManager gameManager;
    private BirdBehaviour birdBehaviour;
    private PlayerController playerController;
    private GameObject saplingInstance;
    private GameObject flowerInstance;
    private GameObject seedInstance;
    public GameObject player;

    void Start()
    {
        plantAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player");
        soilRenderer = GetComponent<Renderer>();

        SetSoilMaterial(soilPatchMaterial);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (seedPlanted && birdBehaviour != null)
        {
            //NB: Moved scare trigger here, instead of in BirdBehaviour, because it has a fun side effect 
            //    of making the birdCawSFX loop and change each time. 
            birdBehaviour.ScareAway();
        }
    }

    // Handle seed planting
    void OnMouseDown()
    {
        if (IsPlayerNearby() && !seedPlanted && playerController.seedCount > 0)
        {
            TryPlantSeed();
        }
    }

    // Handle soil watering
    void OnMouseOver()
    {
        // Check if the right mouse button is clicked
        if (Input.GetMouseButtonDown(1) && IsPlayerNearby() && !isWatered && !isFlower && !isBirdEating && playerController.waterCarried > 0)
        {
            StartCoroutine(WaterPlant());
        }
    }

    void TryPlantSeed()
    {
        if (playerController.seedCount <= 0) return;

        playerController.UpdateSeedCount(-1);
        PlaySound(plantSound);

        AddSeedModel();
        seedPlanted = true;
        StartCoroutine(Stage1_SeededState());
    }

    private IEnumerator Stage1_SeededState()
    {
        if (Random.value < birdChance)
        {
            yield return new WaitForSeconds(birdSpawnTime);
            SpawnBird();
        }

        yield return new WaitForSeconds(stage1_SeededGrowingTime);

        if (seedPlanted)
        {
            StartCoroutine(Stage2_SaplingState());
        }
    }

    private IEnumerator Stage2_SaplingState()
    {
        if (gameManager.gameOver) yield break;

        isSapling = true;

        if (isWatered)
        {
            SetSoilMaterial(wetSoilPatchMaterial);
        }
        else
        {
            SetSoilMaterial(soilPatchMaterial);
        }

        int saplingIndex = Random.Range(0, saplingPrefabs.Length);
        Vector3 spawnPosition = new Vector3(transform.position.x, -0.195f, transform.position.z);
        saplingInstance = Instantiate(saplingPrefabs[saplingIndex], spawnPosition, saplingPrefabs[saplingIndex].transform.rotation);

        if (Random.value < birdChance)
        {
            yield return new WaitForSeconds(birdSpawnTime);
            SpawnBird();
        }

        if (isWatered)
        {
            yield return new WaitForSeconds(stage2_SaplingGrowingTime);

            if (!gameManager.gameOver)
            {
                Stage3_FloweredState();
            }
        }
    }

    private void Stage3_FloweredState()
    {
        if (gameManager.gameOver) return;

        if (!isFlower && !isBirdEating)
        {
            isFlower = true;
            seedPlanted = true;

            if (saplingInstance != null)
            {
                Destroy(saplingInstance.gameObject);
            }

            if (seedInstance != null)
            {
                Destroy(seedInstance.gameObject);
            }

            StopAllCoroutines();

            gameManager.UpdateFlowerCount(1);
            Debug.Log("A flower has grown! Flower count: " + gameManager.flowerCount);

            int flowerIndex = Random.Range(0, flowerPrefabs.Length);
            GameObject selectedPrefab = flowerPrefabs[flowerIndex];

            Vector3 spawnPosition = new Vector3(transform.position.x, -0.35f, transform.position.z);

            // For 'Big' version of flowers, y position changes
            if (selectedPrefab.name.Contains("Big"))
            {
                spawnPosition.y = -0.6f;
            }

            Instantiate(selectedPrefab, spawnPosition, selectedPrefab.transform.rotation);

            gameObject.SetActive(false);
        }
    }

    private IEnumerator WaterPlant()
    {
        if (!seedPlanted)
        {
            isWatered = true;
            playerController.UpdateWaterCarried(-1);
            
            SetSoilMaterial(wetSoilPatchMaterial);
        }
        else if (!isSapling)
        {
            isWatered = true;
            playerController.UpdateWaterCarried(-1);

            SetSoilMaterial(wetSoilPatchMaterial);
        }
        else if (isSapling)
        {
            isWatered = true;
            SetSoilMaterial(wetSoilPatchMaterial);

            yield return new WaitForSeconds(stage2_SaplingGrowingTime);

            Stage3_FloweredState();
        }                   
    }

    private void AddSeedModel()
    {
        seedInstance = Instantiate(seedPrefab, new Vector3(transform.position.x, 0.085f, transform.position.z), seedPrefab.transform.rotation);
    }

    private void SpawnBird()
    {
        if (isBirdEating || gameManager.gameOver)
        {
            return;
        }

        GameObject birdInstance = Instantiate(birdPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), birdPrefab.transform.rotation);
        birdBehaviour = birdInstance.GetComponent<BirdBehaviour>();

        if (birdBehaviour != null)
        {
            birdBehaviour.Initialize(this, player.transform);
        }
    }

    internal void OnBirdAteSeed()
    {
        isBirdEating = false;
        seedPlanted = false;
        isWatered = false;

        if (saplingInstance != null)
        {
            isSapling = false;
            Destroy(saplingInstance.gameObject);
        }

        if (seedInstance != null)
        {
            Destroy(seedInstance.gameObject);
        }

        SetSoilMaterial(soilPatchMaterial);
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
