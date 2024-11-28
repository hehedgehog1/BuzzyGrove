using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyProduction : MonoBehaviour
{
    private bool isMakingHoney = false;
    public float timeToHoney = 25f;
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

    private void Update()
    {   
    }

    public void StartMakingHoney()
    {
        if (!isMakingHoney)
        {
            isMakingHoney = true;
            StartCoroutine(ProduceHoney());
        }
    }

    private IEnumerator ProduceHoney()
    {
        while (!gameManager.gameOver) 
        {
            yield return new WaitForSeconds(timeToHoney);

            int activeFlowers = gameManager.flowerCount;

            gameManager.UpdateHoneyCount(activeFlowers);
            honeyAudio.PlayOneShot(newHoneySound, 1.0f);
            Debug.Log("Honey produced! Total honey: " + gameManager.honeyCount);
        }

        isMakingHoney = false;
    }
}
