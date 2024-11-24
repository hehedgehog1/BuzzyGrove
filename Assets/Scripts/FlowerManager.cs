using System.Collections;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    [Header("Game Objects & Materials")]
    public GameObject[] flowerPrefabs;
    public Material seededMaterial;
    public Material defaultMaterial;
    public GameObject birdPrefab;

    [Header("Audio")]
    public AudioClip plantSound;

    [Header("Timing Settings")]
    private float birdChance = 0.6f;
    private float seedGrowingTime = 60f;
    private float birdSpawnTime = 3f;

    public bool seedPlanted = false;
    private Renderer soilRenderer;
    private AudioSource plantAudio;
    private GameManager gameManager;
    private HoneyProduction honeyProduction;
    private BirdBehaviour birdBehaviour;

    void Start()
    {
        plantAudio = GetComponent<AudioSource>();
        gameManager = gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        honeyProduction = FindObjectOfType<HoneyProduction>();
        soilRenderer = GetComponent<Renderer>();

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
        else if (birdBehaviour != null)
        {
            //NB: Moved scare trigger here, instead of in BirdBehaviour, because it has a fun side effect 
            //    of making the birdCawSFX loop and change each time. 
            birdBehaviour.ScareAway();
        }
    }

    private void TryPlantSeed(Collider other)
    {
        if (gameManager.seedCount <= 0) return;

        gameManager.UpdateSeedCount(-1);
        PlaySound(plantSound);
        SetSoilMaterial(seededMaterial);
        seedPlanted = true;
        StartCoroutine(SeededState());
    }

    private IEnumerator SeededState()
    {
        if (Random.value < birdChance)
        {
            Debug.Log("A bird will appear");
            yield return new WaitForSeconds(birdSpawnTime);
            SpawnBird();
        }
        Debug.Log("seed is growing");
        yield return new WaitForSeconds(seedGrowingTime);

        if (seedPlanted)
        {
            FloweredState();
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
        //potentially re-add birdEating = true here
        seedPlanted = false;
        SetSoilMaterial(defaultMaterial);
    }

    private void FloweredState()
    {
        SetSoilMaterial(defaultMaterial);

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
