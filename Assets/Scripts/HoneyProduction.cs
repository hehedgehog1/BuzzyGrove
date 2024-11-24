using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyProduction : MonoBehaviour
{
    public float timeToHoney = 20f;
    private const float minTimeToHoney = 5f;
    private const float timeReduction = 5f; // 5s time reduction    
    private GameManager gameManager;

    public AudioClip newHoneySound;
    private AudioSource honeyAudio;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        honeyAudio = GetComponent<AudioSource>();
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

        if (!gameManager.gameOver)
        {
            gameManager.UpdateHoneyCount(1);
            honeyAudio.PlayOneShot(newHoneySound, 1.0f);
            Debug.Log("Honey produced! Total honey: " + gameManager.honeyCount);
        }
    }
}
