using System.Collections;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public AudioClip birdCawSound;

    private float birdEatingTime = 15f;
    private float hostileChance = 1f;
    private FlowerManager flowerManager;
    private AudioSource audioSource;
    private Transform player;
    private PlayerController playerController;
    private bool isHostile;
    private float chaseSpeed = 8f;
    private bool isChasingPlayer = false;
    private bool canCatch = false;
    public float safeWindow = 0.5f;

    private bool isEating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void Initialize(FlowerManager fm, Transform playerTransform)
    {
        flowerManager = fm;
        isHostile = Random.Range(0f, 1f) < hostileChance;
        player = playerTransform;
        StartCoroutine(BirdWaitThenEat());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isChasingPlayer == true && canCatch == true)
        {
            StartCoroutine(playerController.CaughtByBird());
            StartCoroutine(BirdFliesAway());            
        }
    }

    public void ScareAway()
    {
        PlaySound(birdCawSound);

        isEating = false;
        flowerManager.isBirdEating = false;

        if (isHostile)
        {
            StopAllCoroutines();
            StartCoroutine(ChasePlayer());
        }
        else
        {
            StopAllCoroutines();
            StartCoroutine(BirdFliesAway());
        }             
    }

    private IEnumerator ChasePlayer()
    {
        canCatch = false;
        
        yield return new WaitForSeconds(safeWindow);

        isChasingPlayer = true;
        canCatch = true;
        
        float chaseTime = 10f;
        float elapsedTime = 0f;

        while (elapsedTime < chaseTime)
        {
            Vector3 targetPosition = new Vector3(player.position.x, 2.9f, player.position.z);

            transform.position = Vector3.MoveTowards(transform.position, targetPosition, chaseSpeed * Time.deltaTime);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        isChasingPlayer = false;
        StartCoroutine(BirdFliesAway());
    }

    private IEnumerator BirdWaitThenEat()
    {
        isEating = true;
        flowerManager.isBirdEating = true;

        Debug.Log("A bird has appeared!");

        yield return new WaitForSeconds(birdEatingTime);

        if (flowerManager.seedPlanted && isEating)
        {
            Debug.Log("A bird ate a seed!");

            flowerManager.OnBirdAteSeed();
            Destroy(gameObject);
        }
    }

    private IEnumerator BirdFliesAway()
    {        
        Vector3 startPosition = transform.position;
        Vector3 offScreenPosition = new Vector3(60f, 10f, 10f);
        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, offScreenPosition, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip, 1.0f);
        }
    }
}
