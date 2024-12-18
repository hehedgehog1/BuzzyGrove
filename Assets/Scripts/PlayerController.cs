using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    public float speed = 9f;
    public int waterCarried = 0;
    public int seedCount = 0;    
    public bool isStunned = false;
    public float interactionRadius = 5.0f;

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
        waterCarried += amount;        
        uiManager.UpdateWaterText();

        CalculateSpeed();
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
        // speed = 3f;

        //yield return new WaitForSeconds(3f);

        //isStunned = false;
        //Debug.Log("not stunned");
        //CalculateSpeed();
    }

    void CalculateSpeed()
    {
        if (!isStunned)
        {
            //change speed 7-9 depending on how much water carried
            if (waterCarried <= 1)
            {
                speed = 9f;
            }
            else if (waterCarried > 1 && waterCarried < 5)
            {
                speed = 8f;
            }
            else
            {
                speed = 7f;
            }
        }        
    }
}
