using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyProduction : MonoBehaviour
{
    private bool isMakingHoney = false;
    public float timeToHoney = 5f; 
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

    public IEnumerator MakeHoney()
    {
        Debug.Log("Making honey...");
        yield return new WaitForSeconds(timeToHoney);
        Debug.Log("Honey making finished...");
        gameManager.UpdateHoneyCount(1);
    }

    //public void StartMakingHoney()
    //{
    //    if (!isMakingHoney)
    //    {
    //        isMakingHoney = true;
    //        StartCoroutine(ProduceHoney());
    //    }
    //}
    //
    //private IEnumerator ProduceHoney()
    //{
    //    while (!gameManager.gameOver) 
    //    {
    //        yield return new WaitForSeconds(timeToHoney);

    //        int activeFlowers = gameManager.flowerCount;

    //        if (activeFlowers == 0)
    //        {
    //            continue;
    //        }
    //        else if (activeFlowers > 9)
    //        {
    //            gameManager.UpdateHoneyCount(activeFlowers/3);
    //            honeyAudio.PlayOneShot(newHoneySound, 1.0f);
    //        }
    //        else
    //        {
    //            gameManager.UpdateHoneyCount(activeFlowers);
    //            honeyAudio.PlayOneShot(newHoneySound, 1.0f);
    //        }    
    //    }

    //    isMakingHoney = false;
    //}
}
