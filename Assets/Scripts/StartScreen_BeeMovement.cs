using UnityEngine;

public class StartScreen_BeeMovement : MonoBehaviour
{
    public float amplitude = 0.5f; // How high the bee moves up and down.
    public float frequency = 1f;  // How fast the bee moves up and down.

    private Vector3 startPos;
    //private GameManager gameManager;

    void Start()
    {
        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        startPos = transform.position;
    }

    void Update()
    {
        //if (!gameManager.gameOver)
        //{
            // Move bee up and down
            float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
            transform.position = new Vector3(startPos.x, startPos.y + yOffset, startPos.z);
        //}            
    }
}
