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

    void Update()
    {
    }


    private void OnMouseDown()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.gameObject == gameObject)
            {
                float distance = Vector3.Distance(playerController.transform.position, hit.point);

                if (distance <= interactionRadius)
                {
                    Debug.Log("Water clicked");

                    if (playerController.waterCarried < 5)
                    {
                        PlaySound(waterSound);
                        Debug.Log("Water updated by 1");
                        playerController.UpdateWaterCarried(1);
                    }
                }
                else { Debug.Log("Player too far away to click"); }
            }
        }
    }

    //    private bool IsPlayerNearby()
    //{
    //    Collider[] colliders = Physics.OverlapSphere(transform.position, interactionRadius);
    //    foreach (Collider collider in colliders)
    //    {
    //        if (collider.CompareTag("Player"))
    //        {
    //            return true;
    //        }
    //    }
    //    Debug.Log("Player too far away to click");
    //    return false;
    //}

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null && !audioSource.isPlaying)
        {
            audioSource.PlayOneShot(clip, 1.0f);
        }
    }
}
