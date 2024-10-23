using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int seedCount = 0;
    public int flowerCount = 0;
    public int honeyCount = 0;
    public float dayTimer = 350f;
    private bool gameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (!gameOver)
        {
            // Timer logic
            if (dayTimer > 0)
            {
                dayTimer -= Time.deltaTime; // Decrement timer by the time passed since last frame
                Debug.Log("current time: " + dayTimer);
            }
            else
            {
                dayTimer = 0;
                GameOver();
            }

            // Movement logic
            float moveX = -Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            float moveZ = -Input.GetAxis("Vertical") * Time.deltaTime * speed;

            Vector3 move = new Vector3(moveX, 0, moveZ);
            transform.Translate(move);
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Seed"))
        {
            seedCount++;
            Debug.Log("New seed collected. Total seeds: " + seedCount);
        }
    }

    private void GameOver()
    {
        gameOver = true;
        Debug.Log("Day has ended! HoneyCount = " + honeyCount);
    }
}
