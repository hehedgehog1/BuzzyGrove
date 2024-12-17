using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform player; 
    public Vector3 offset;

    void Start()
    {
        // Calculate the initial offset between the player and camera
        offset = transform.position - player.position;
    }

    void Update()
    {
        transform.position = player.position + offset;
    }
}
