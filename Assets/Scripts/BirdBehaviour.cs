using System.Collections;
using UnityEngine;

public class BirdBehaviour : MonoBehaviour
{
    public AudioClip birdCawSound;

    private float birdEatingTime = 20f;
    private FlowerManager flowerManager;
    private AudioSource audioSource;

    private bool isEating = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Initialize(FlowerManager fm)
    {
        flowerManager = fm;
        StartCoroutine(BirdWaitThenEat());
    }

    public void ScareAway()
    {
        PlaySound(birdCawSound);
        isEating = false;
        flowerManager.isBirdEating = false;
        StopAllCoroutines();
        StartCoroutine(BirdFliesAway());
        
    }

    private IEnumerator BirdWaitThenEat()
    {
        isEating = true;
        flowerManager.isBirdEating = true;
        Debug.Log("A bird has appeared!");

        yield return new WaitForSeconds(birdEatingTime);

        if (flowerManager.seedPlanted && isEating)
        {
            Debug.Log("A bird ate a seed!");
            flowerManager.OnBirdAteSeed();
            Destroy(gameObject);
        }
    }

    private IEnumerator BirdFliesAway()
    {
        Vector3 startPosition = transform.position;
        Vector3 offScreenPosition = new Vector3(60f, 10f, 10f);
        float duration = 1.0f;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            transform.position = Vector3.Lerp(startPosition, offScreenPosition, t);
            yield return null;
        }

        Destroy(gameObject);
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip, 1.0f);
        }
    }
}
