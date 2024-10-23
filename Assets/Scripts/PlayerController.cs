using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public int seedCount = 0;
    public int flowerCount = 0;
    public int honeyCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float moveX = -Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        float moveZ = -Input.GetAxis("Vertical") * Time.deltaTime * speed;

        Vector3 move = new Vector3(moveX, 0, moveZ);
        transform.Translate(move);
    }

    void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Seed"))
        {
            seedCount++;
            Debug.Log("New seed collected. Total seeds: " + seedCount);
        }

        if (collision.gameObject.CompareTag("SoilPatch") && seedCount > 0)
        {
            seedCount--;
            flowerCount++;
        }
    }
}
