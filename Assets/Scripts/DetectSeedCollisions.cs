using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedCollider : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        //When player collides with seed
        Destroy(gameObject);
    }
}
