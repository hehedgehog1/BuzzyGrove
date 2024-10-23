using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyProduction : MonoBehaviour
{
    public float timeToHoney = 20f;
    private const float minTimeToHoney = 5f;
    private const float timeReduction = 5f; // 5s time reduction    
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = FindObjectOfType<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void StartMakingHoney()
    {
        //TODO: Honey is only made once per flower. This should probably be continuously while flowers exist.
        Debug.Log("Honey Started");
        StartCoroutine(ProduceHoney());

        //// Time taken to produce honey is reduced per flower added
        //timeToHoney *= (1 - timeReduction);

        //// Minimum time is 5 seconds
        //timeToHoney = Mathf.Max(timeToHoney, minTimeToHoney);

        //Debug.Log("Honey production time now: " + timeToHoney + " seconds");
    }

    private IEnumerator ProduceHoney()
    {
        yield return new WaitForSeconds(timeToHoney);

        if (playerController != null)
        {
            playerController.honeyCount++;
            Debug.Log("Honey produced! Total honey: " + playerController.honeyCount);
        }
        else
        {
            Debug.LogError("PlayerController reference not found!");
        }
    }
}
