using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyProduction : MonoBehaviour
{
    public float timeToHoney = 7f; 
    private GameManager gameManager;

    public AudioClip newHoneySound;
    private AudioSource honeyAudio;

    void Start()
    {
        gameManager = gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        honeyAudio = GetComponent<AudioSource>();
    }

    public IEnumerator MakeHoney()
    {
        if (gameManager.gameOver) yield break;

        Debug.Log("Making honey...");
        yield return new WaitForSeconds(timeToHoney);
        Debug.Log("Honey making finished...");

        honeyAudio.PlayOneShot(newHoneySound, 1.0f);
        gameManager.UpdateHoneyCount(1);
    }
}
