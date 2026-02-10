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

    /// <summary>
    /// Updates camera position after all other updates to prevent jitter.
    /// Maintains constant offset from player.
    /// </summary>  
    void LateUpdate()
    {
        transform.position = player.position + offset;
    }
}
