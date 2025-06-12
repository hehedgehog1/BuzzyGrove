using System.Collections;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public AudioClip birdCawSound;

    private const float BirdEatingTime = 15f;
    private const float HostileChance = 0.3f;
    private const float ChaseTime = 8f;
    private const float ChaseSpeed = 5f; //original = 6
    private const float MinDistanceToPlayer = 0.5f;
    private const float SafeWindow = 0.2f;

    private SoilManager soilManager;
    private AudioSource audioSource;
    private Transform player;
    private PlayerController playerController;

    private bool isHostile;
    private bool isChasingPlayer = false;
    private bool canCatch = false;
    private bool isEating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    public void Initialize(SoilManager soilManager, Transform playerTransform)
    {
        this.soilManager = soilManager;
        isHostile = Random.Range(0f, 1f) < HostileChance;
        player = playerTransform;
        StartCoroutine(BirdWaitThenEat());
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && isChasingPlayer && canCatch)
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

        yield return new WaitForSeconds(SafeWindow); //so bird doesn't instantly catch player

        canCatch = true;                
        float elapsedTime = 0f;

        while (elapsedTime < ChaseTime)
        {
            Vector3 targetPosition = new Vector3(player.position.x, 2.9f, player.position.z);
            Vector3 directionToPlayer = targetPosition - transform.position;

            // If the bird is too close to the player, assume player is caught
            if (directionToPlayer.magnitude < MinDistanceToPlayer)
            {
                playerController.CaughtByBird();
                StartCoroutine(BirdFliesAway());
                yield break;
            }

            // Rotate bird to face the direction it's moving
            if (directionToPlayer != Vector3.zero)
            {
                Vector3 flatDirection = new Vector3(directionToPlayer.x, 0f, directionToPlayer.z);
                Quaternion targetRotation = Quaternion.LookRotation(flatDirection);

                // Apply a 90-degree rotation offset if the model faces +X instead of +Z
                Quaternion rotationOffset = Quaternion.Euler(-90f, -90f, 0f); // adjust angle as needed
                Quaternion finalRotation = targetRotation * rotationOffset;

                transform.rotation = Quaternion.Slerp(transform.rotation, finalRotation, 10f * Time.deltaTime);
            }


            transform.position = Vector3.MoveTowards(transform.position, targetPosition, ChaseSpeed * Time.deltaTime);

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

        yield return new WaitForSeconds(BirdEatingTime);

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
