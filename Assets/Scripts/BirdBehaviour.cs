using System.Collections;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public AudioClip birdCawSound;

    private float birdEatingTime = 15f;
    private float hostileChance = 0.3f;
    private float chaseTime = 8f;
    private SoilManager soilManager;
    private AudioSource audioSource;
    private Transform player;
    private PlayerController playerController;
    private bool isHostile;
    private float chaseSpeed = 6f;
    private bool isChasingPlayer = false;
    private bool canCatch = false;
    private float safeWindow = 0.2f; 

    private bool isEating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void Initialize(SoilManager fm, Transform playerTransform)
    {
        soilManager = fm;
        isHostile = Random.Range(0f, 1f) < hostileChance;
        player = playerTransform;
        StartCoroutine(BirdWaitThenEat());
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (isChasingPlayer == true && canCatch == true)
        {
            playerController.CaughtByBird();
            StartCoroutine(BirdFliesAway());
        }
    }

    public void ScareAway()
    {
        PlaySound(birdCawSound);

        isEating = false;
        soilManager.isBirdEating = false;

        StopAllCoroutines();

        if (isHostile)
        {
            Debug.Log("Bird is hostile");
            StartCoroutine(ChasePlayer());
        }
        else
        {
            StartCoroutine(BirdFliesAway());
        }
    }

    private IEnumerator ChasePlayer()
    {
        isChasingPlayer = true;
        canCatch = false;

        yield return new WaitForSeconds(safeWindow);

        canCatch = true;
                
        float elapsedTime = 0f;

        float minDistanceToPlayer = 0.5f;

        while (elapsedTime < chaseTime)
        {
            Vector3 targetPosition = new Vector3(player.position.x, 2.9f, player.position.z);

            Vector3 directionToPlayer = targetPosition - transform.position;

            // If the bird is too close to the player, assume player is caught
            if (directionToPlayer.magnitude < minDistanceToPlayer)
            {
                playerController.CaughtByBird();
                StartCoroutine(BirdFliesAway());
            }

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
        soilManager.isBirdEating = true;

        Debug.Log("A bird has appeared!");

        yield return new WaitForSeconds(birdEatingTime);

        if (soilManager.seedPlanted && isEating)
        {
            Debug.Log("A bird ate a seed!");

            soilManager.OnBirdAteSeed();
            Destroy(gameObject);
        }
    }

    private IEnumerator BirdFliesAway()
    {
        Vector3 startPosition = transform.position;
        Vector3 offScreenPosition = new Vector3(70f, 30f, 10f);
        float duration = 2.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;

            transform.position = Vector3.Lerp(startPosition, offScreenPosition, t);

            // Rotate the bird to face the direction it's movin
            Vector3 direction = offScreenPosition - transform.position;
            if (direction.sqrMagnitude > 0.01f)  // Avoid issues with zero magnitude
            {
                Quaternion targetRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);  // Adjust speed of rotation
            }

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
