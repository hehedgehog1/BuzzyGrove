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
    public GameObject birdPrefab;

    [Header("Audio")]
    public AudioClip plantSound;

    [Header("Timing Settings")]
    private float birdChance = 0.6f;
    private float stage1_SeededGrowingTime = 10f; //TODO changed for testing purposes
    private float stage2_SaplingGrowingTime = 10f; // TODO changed for testing purposes
    private float birdSpawnTime = 3f;

    public bool seedPlanted = false;
    public bool isSapling = false;
    public bool isWatered = false;
    public bool isBirdEating = false;

    private Renderer soilRenderer;
    private AudioSource plantAudio;
    private GameManager gameManager;
    private HoneyProduction honeyProduction;
    private BirdBehaviour birdBehaviour;
    private PlayerController playerController;
    private GameObject saplingInstance;

    void Start()
    {
        plantAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        honeyProduction = FindObjectOfType<HoneyProduction>();
        soilRenderer = GetComponent<Renderer>();

        SetSoilMaterial(soilPatchMaterial);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        // if player touches unseeded soil:
        if (!seedPlanted)
        {
            TryPlantSeed(other);
        }
        else if (birdBehaviour != null)
        {
            //NB: Moved scare trigger here, instead of in BirdBehaviour, because it has a fun side effect 
            //    of making the birdCawSFX loop and change each time. 
            birdBehaviour.ScareAway();
        }
        if (seedPlanted && playerController.waterCarried > 0)
        {
            WaterPlant();
        }
    }

    private void TryPlantSeed(Collider other)
    {
        if (gameManager.seedCount <= 0) return;

        gameManager.UpdateSeedCount(-1);
        PlaySound(plantSound);
        SetSoilMaterial(seededMaterial);
        seedPlanted = true;
        StartCoroutine(Stage1_SeededState()); //TODO this shouldnt be coroutine, call CR within method
    }

    private IEnumerator Stage1_SeededState()
    {
        if (Random.value < birdChance)
        {
            Debug.Log("Yes bird");
            yield return new WaitForSeconds(birdSpawnTime);
            SpawnBird();
        }
        Debug.Log("No bird");

        yield return new WaitForSeconds(stage1_SeededGrowingTime);
        Debug.Log("done stage 1");

        if (seedPlanted)
        {
            StartCoroutine(Stage2_SaplingState());
        }
    }

    private IEnumerator Stage2_SaplingState()
    {
        Debug.Log("in stage 2");

        isSapling = true;
        SetSoilMaterial(soilPatchMaterial);

        int saplingIndex = Random.Range(0, saplingPrefabs.Length);
        saplingInstance = Instantiate(saplingPrefabs[saplingIndex], transform.position, Quaternion.identity);

        if (Random.value < birdChance)
        {
            Debug.Log("yes bird stage 2");
            yield return new WaitForSeconds(birdSpawnTime);
            SpawnBird();
        }
        Debug.Log("no bird stage 2");

        //if (seedPlanted && isSapling && isWatered)
        //{
        //    yield return new WaitForSeconds(stage2_SaplingGrowingTime);
        //    Stage3_FloweredState();
        //}
    }

    private IEnumerator WaterPlant()
    {
        if (isSapling == false)
        {
            isWatered = true;
            SetSoilMaterial(wetSeededMaterial);
        }
        else if (isSapling == true)
        {
            isWatered = true;
            SetSoilMaterial(wetSoilPatchMaterial);
            yield return new WaitForSeconds(stage2_SaplingGrowingTime);
            Stage3_FloweredState();
        }      
    }

    private void SpawnBird()
    {
        GameObject birdInstance = Instantiate(birdPrefab, new Vector3(transform.position.x, 0.5f, transform.position.z), Quaternion.identity);

        birdBehaviour = birdInstance.GetComponent<BirdBehaviour>();
        if (birdBehaviour != null)
        {
            birdBehaviour.Initialize(this);
        }
    }

    public void OnBirdAteSeed()
    {
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
        Destroy(saplingInstance.gameObject);
        SetSoilMaterial(null);

        gameManager.UpdateFlowerCount(1);
        Debug.Log("A flower has grown! Flower count: " + gameManager.flowerCount);

        int flowerIndex = Random.Range(0, flowerPrefabs.Length);
        Instantiate(flowerPrefabs[flowerIndex], transform.position, Quaternion.identity);

        honeyProduction.StartMakingHoney();
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
