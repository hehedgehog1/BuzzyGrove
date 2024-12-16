using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerManager : MonoBehaviour
{
    private HoneyProduction honeyProduction;

    // Start is called before the first frame update
    void Start()
    {
        honeyProduction = FindObjectOfType<HoneyProduction>();
        StartCoroutine(honeyProduction.MakeHoney());
    }
}
