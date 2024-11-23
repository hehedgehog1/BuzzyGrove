using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int seedCount = 0;
    public int flowerCount = 0;
    public int honeyCount = 0;
    public float dayTimer = 300f; //5 mins
    public string timeOfDay = "";
    public AudioClip seedPickUpSound;
    private AudioSource playerAudio;
    public bool gameOver = false;
    private float xRange = 50.0f;
    private float zRange = 50.0f;
    private UIManager uiManager;


    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        uiManager = FindObjectOfType<UIManager>();
    }

    void Update()
    {
        updateDayLeft();

        if (!gameOver)
        {
            // Timer logic
            if (dayTimer > 0)
            {
                dayTimer -= Time.deltaTime; // Decrement timer by the time passed since last frame
                //Debug.Log("current time: " + dayTimer);
            }
            else
            {
                dayTimer = 0;
                GameOver();
            }

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

    private void updateDayLeft()
    {
        if (dayTimer > 240f)
        {
            timeOfDay = "Morning";
            uiManager.UpdateTimeOfDayText();
        }
        else if (dayTimer > 120f)
        {
            timeOfDay = "Afternoon";
            uiManager.UpdateTimeOfDayText();
        }
        else if (gameOver)
        {
            timeOfDay = "Goodnight!";
            uiManager.UpdateTimeOfDayText();
        }
        else
        {
            timeOfDay = "Evening!";
            uiManager.UpdateTimeOfDayText();
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

    public void UpdateSeedCount(int seedToAdd)
    {
        seedCount += seedToAdd;
        uiManager.UpdateSeedText();
    }

    public void UpdateHoneyCount(int honeyToAdd)
    {
        honeyCount += honeyToAdd;
        uiManager.UpdateHoneyText();
    }

    private void GameOver()
    {
        gameOver = true;
        Debug.Log("Day has ended! HoneyCount = " + honeyCount);
    }
}
