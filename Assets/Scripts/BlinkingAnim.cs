using System.Collections;
using UnityEngine;

public class Blink : MonoBehaviour
{
    public Renderer bodyRenderer;
    public Texture eyesOpen;
    public Texture eyesClosed;

    void Start()
    {
        StartCoroutine(BlinkRoutine());
    }

    IEnumerator BlinkRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 5f));

            bodyRenderer.material.mainTexture = eyesClosed;

            yield return new WaitForSeconds(0.1f);

            bodyRenderer.material.mainTexture = eyesOpen;
        }
    }
}