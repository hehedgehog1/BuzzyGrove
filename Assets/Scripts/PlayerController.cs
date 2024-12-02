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

    private float xRange = 50.0f;
    private float zRange = 50.0f;

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

                Debug.Log("here1");
                speed = 9f;
            }
            else if (waterCarried > 1 && waterCarried < 5)
            {

                Debug.Log("here2");
                speed = 8f;
            }
            else
            {

                Debug.Log("here3");
                speed = 7f;
            }
        }        
    }
}
