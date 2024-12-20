using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterSourceBehaviour : MonoBehaviour
{
    public float interactionRadius;
    private PlayerController playerController;
    public AudioClip waterSound;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        Debug.Log("Water clicked");
        if (IsPlayerNearby() && playerController.waterCarried < 5)
        {
            PlaySound(waterSound);
            Debug.Log("Water updated by 1");
            playerController.UpdateWaterCarried(1);
        }
    }

    private bool IsPlayerNearby()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                return true;
            }
        }
        Debug.Log("Player too far away to click");
        return false;
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip, 1.0f);
        }
    }
}
