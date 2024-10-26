using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject soilPatchPrefab;
    public float speed = 5f;
    public int seedCount = 0;
    public int flowerCount = 0;
    public int honeyCount = 0;
    public float dayTimer = 350f;
    private bool gameOver = false;
    private Camera mainCamera;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
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

            //Clicking logic
            HandleMouseClick();

            // Movement logic

            Rigidbody rb = GetComponent<Rigidbody>();

            float moveX = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            float moveZ = Input.GetAxis("Vertical") * Time.deltaTime * speed;

            Vector3 move = new Vector3(moveX, 0, moveZ);
            rb.MovePosition(rb.position + move);

            // Maintain upright rotation
            transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
        }
    }

    void HandleMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 soilPos = transform.position;
            //TODO: check valid soil pos
            soilPos.z -= 1f;
            soilPos.y = 0.03f;
            Instantiate(soilPatchPrefab, soilPos, Quaternion.identity);


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
