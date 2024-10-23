using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;

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
}
