using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    private float speed = 7f;
    public int waterCarried = 0;
    public int seedCount = 0;    
    public bool isStunned = false;
    public float interactionRadius = 5.0f;
    private float stunLength = 10.0f;
    private float stunSpeed = 2.0f;

    private float xRange = 58.0f;
    private float zRange = 58.0f;

    public Animator anim;
    public AudioClip seedPickUpSound;
    private AudioSource playerAudio;
    private GameManager gameManager;
    private UIManager uiManager;

    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {      
        if (!gameManager.gameOver)
        {
            // Movement logic
            Rigidbody rb = GetComponent<Rigidbody>();

            //Stop player getting too fast
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            
            // out of bounds handling
            if (rb.position.x < -xRange)
            {
                rb.position = new Vector3(-xRange, rb.position.y, rb.position.z);
            }
            if (rb.position.x > xRange)
            {
                rb.position = new Vector3(xRange, rb.position.y, rb.position.z);
            }
            if (rb.position.z < -zRange)
            {
                rb.position = new Vector3(rb.position.x, rb.position.y, -zRange);
            }
            if (rb.position.z > zRange)
            {
                rb.position = new Vector3(rb.position.x, rb.position.y, zRange);
            }

            //Updates animator
            float moveX = Input.GetAxis("Horizontal");
            float moveZ = Input.GetAxis("Vertical");

            Vector3 move = new Vector3(moveX, 0, moveZ);
            move.Normalize();

            //update animator params
            anim.SetFloat("horizontal", moveX);
            anim.SetFloat("vertical", moveZ);
                        
            if (move.magnitude > 0.01f)
            {
                Vector3 movement = move * speed * Time.deltaTime;
                rb.MovePosition(rb.position + movement);

                //rotates character to face walking direction
                Quaternion targetRotation = Quaternion.LookRotation(-move);
                rb.rotation = Quaternion.Slerp(rb.rotation, targetRotation, 0.1f);
            }
        }
        else
        {
            //when game over, player should stop walking
            anim.SetFloat("horizontal", 0);
            anim.SetFloat("vertical", 0);
            Debug.Log("Game over - animation reset");
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
        isStunned = true;
        Debug.Log("is stunned");
        StartCoroutine(TemporarySpeedChange(stunSpeed, stunLength));
    }

    private IEnumerator TemporarySpeedChange(float newSpeed, float duration)
    {
        float originalSpeed = speed;
        speed = newSpeed;

        yield return new WaitForSeconds(duration);

        speed = originalSpeed;
        isStunned = false;
    }


}
