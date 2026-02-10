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

    // LateUpdate runs after everything else, including player movement
    // This means the camera always follows the player's final position each frame
    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
