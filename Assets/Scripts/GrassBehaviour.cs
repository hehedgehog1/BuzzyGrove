using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassBehaviour : MonoBehaviour
{
    public GameObject soilPrefab;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        //TODO: ideally, should only be able to click on grass when near it
        Instantiate(soilPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
