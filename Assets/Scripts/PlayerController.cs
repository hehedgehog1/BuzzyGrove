using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int waterCarried = 0;
    
    public AudioClip seedPickUpSound;
    private AudioSource playerAudio;
    
    private float xRange = 50.0f;
    private float zRange = 50.0f;
    private GameManager gameManager;


    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {      
        if (!gameManager.gameOver)
        {
            // Movement logic
            Rigidbody rb = GetComponent<Rigidbody>();

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

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            Vector3 move = new Vector3(moveX, 0, moveZ);
            rb.MovePosition(rb.position + move);

            // Maintain upright rotation
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }        
    }    

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Seed"))
        {
            playerAudio.PlayOneShot(seedPickUpSound, 1.0f);
            gameManager.UpdateSeedCount(1);            
        }
    }

    internal void UpdateWaterCarried(int amount)
    {
        waterCarried += amount;
    }
}
