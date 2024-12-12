using System.Collections;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    [Header("Game Objects & Materials")]
    public GameObject[] saplingPrefabs;
    public GameObject[] flowerPrefabs;
    public Material seededMaterial;
    public Material soilPatchMaterial;
    public Material wetSeededMaterial;
    public Material wetSoilPatchMaterial;
    public Material GroundMaterial;
    public GameObject birdPrefab;

    [Header("Audio")]
    public AudioClip plantSound;

    [Header("Timing Settings")]
    private float birdChance = 0.6f;
    private float stage1_SeededGrowingTime = 40f;
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
    private HoneyProduction honeyProduction;
    private BirdBehaviour birdBehaviour;
    private PlayerController playerController;
    private GameObject saplingInstance;
    private GameObject flowerInstance;
    public GameObject player;

    void Start()
    {
        plantAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        player = GameObject.Find("Player");
        honeyProduction = FindObjectOfType<HoneyProduction>();
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

    void OnMouseDown()
    {
        if (!seedPlanted && playerController.seedCount > 0)
        {
            TryPlantSeed();
        }
    }

    void OnMouseOver()
    {
        // Check if the right mouse button is clicked
        if (Input.GetMouseButtonDown(1) && !isWatered && !isFlower && playerController.waterCarried > 0)
        {
            StartCoroutine(WaterPlant());
        }
    }

    void TryPlantSeed()
    {
        if (playerController.seedCount <= 0 || IsSpaceOccupied()) return;

        playerController.UpdateSeedCount(-1);
        PlaySound(plantSound);

        if (isWatered)
        {
            SetSoilMaterial(wetSeededMaterial);
        }
        else
        {
            SetSoilMaterial(seededMaterial);
        }

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

        if (!IsSpaceOccupied()) //Check space is clear before spawning the sapling
        {
            int saplingIndex = Random.Range(0, saplingPrefabs.Length);
            saplingInstance = Instantiate(saplingPrefabs[saplingIndex], transform.position, Quaternion.identity);
        }

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
            
            SetSoilMaterial(wetSeededMaterial);
        }
        else if (isSapling)
        {
            isWatered = true;
            SetSoilMaterial(wetSoilPatchMaterial);

            yield return new WaitForSeconds(stage2_SaplingGrowingTime);

            Stage3_FloweredState();
        }                   
    }

    private void SpawnBird()
    {
        if (isBirdEating || gameManager.gameOver)
        {
            return;
        }

        GameObject birdInstance = Instantiate(birdPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);
        birdBehaviour = birdInstance.GetComponent<BirdBehaviour>();

        if (birdBehaviour != null)
        {
            birdBehaviour.Initialize(this, player.transform);
        }
    }

    public void OnBirdAteSeed()
    {
        isBirdEating = false;
        seedPlanted = false;

        if (saplingInstance != null)
        {
            isSapling = false;
            Destroy(saplingInstance.gameObject);
        }

        SetSoilMaterial(soilPatchMaterial);
    }

    private void Stage3_FloweredState()
    {
        if (gameManager.gameOver) return;

        if (!isFlower && !IsSpaceOccupied()) 
        {
            isFlower = true;
            seedPlanted = true;

            if (saplingInstance != null)
            {
                Destroy(saplingInstance.gameObject);
            }
            StopAllCoroutines();

            SetSoilMaterial(GroundMaterial);

            gameManager.UpdateFlowerCount(1);
            Debug.Log("A flower has grown! Flower count: " + gameManager.flowerCount);

            int flowerIndex = Random.Range(0, flowerPrefabs.Length);
            Instantiate(flowerPrefabs[flowerIndex], new Vector3(transform.position.x, 0.8f, transform.position.z), Quaternion.identity);

            StartCoroutine(honeyProduction.MakeHoney());
        }
    }

    private bool IsSpaceOccupied()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.5f);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Flower") || collider.CompareTag("Sapling") || collider.CompareTag("Soil"))
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
