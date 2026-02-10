using System.Collections;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private AudioClip seedPickUpSound;

    public int waterCarried = 0;
    public int seedCount = 0;
    public bool isStunned = false;

    [Tooltip("Range at which player can interact with objects")]
    [SerializeField] public float interactionRadius = 5.0f;

    [SerializeField] private float speed = 7f;    
    [SerializeField] private float stunLength = 8.0f;
    [SerializeField] private float stunSpeed = 2.0f;
    [SerializeField] private float xRange = 58.0f;
    [SerializeField] private float zRange = 58.0f;

    public Animator anim;

    private Rigidbody rb;
    private AudioSource playerAudio;
    private GameManager gameManager;
    private UIManager uiManager;
    private Vector3 moveInput;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    
    void Update()
    {
        if (!gameManager.gameOver)
        {
            // Get input
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");
            moveInput = new Vector3(moveX, 0, moveZ).normalized;

            // Update animator
            anim.SetFloat("horizontal", moveX);
            anim.SetFloat("vertical", moveZ);
        }
        else
        {
            moveInput = Vector3.zero;
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
            Debug.Log("Game over - animation reset");
        }
    }

/// <summary>
/// Handles physics-based player movement with smooth acceleration and deceleration.
/// Uses velocity instead of MovePosition for more natural feel.
/// </summary>
void FixedUpdate()
{
    if (!gameManager.gameOver)
    {
        // Clamp position within bounds
        float clampedX = Mathf.Clamp(rb.position.x, -xRange, xRange);
        float clampedZ = Mathf.Clamp(rb.position.z, -zRange, zRange);
        rb.position = new Vector3(clampedX, rb.position.y, clampedZ);

        // Only move if input exists
        if (moveInput.magnitude > 0.01f)
        {
            // Use velocity instead of MovePosition for smoother, physics-based movement
            Vector3 targetVelocity = moveInput * speed;
            rb.velocity = Vector3.Lerp(rb.velocity, targetVelocity, 0.1f);

            // Rotate to face movement direction
            Quaternion targetRotation = Quaternion.LookRotation(-moveInput);
            rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 0.15f);
        }
        else
        {
            // Decelerate smoothly when no input
            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, 0.4f);
        }
        
        // Keep the bee grounded (prevent vertical drift)
        rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
    }
    else
    {
        // Stop all movement when game is over
        rb.velocity = Vector3.zero;
    }
}

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Seed"))
        {
            playerAudio.PlayOneShot(seedPickUpSound, 1.0f);
            UpdateSeedCount(1);            
        }
    }

    internal void UpdateWaterCarried(int amount)
    {
        // Update water carried
        waterCarried += amount;    
        uiManager.UpdateWaterText();
    }

    internal void UpdateSeedCount(int seedToAdd)
    {
        seedCount += seedToAdd;
        uiManager.UpdateSeedText();
    }

    internal void CaughtByBird()
    {
        if (!isStunned)
        {
            isStunned = true;
            StartCoroutine(TemporarySpeedChange(stunSpeed, stunLength));
        }
        else
        {
            Debug.Log("Player is already stunned; refreshing stun duration.");
        }
    }

    private IEnumerator TemporarySpeedChange(float newSpeed, float duration)
    {
        float originalSpeed = speed;
        speed = newSpeed;

        yield return new WaitForSeconds(duration);

        // Check if another stun effect is active before resetting
        if (speed == newSpeed)
        {
            speed = originalSpeed;
            isStunned = false;
            Debug.Log("Player is no longer stunned.");
        }
    }
}
