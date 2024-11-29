using UnityEngine;
using UnityEngine.UI;

public class StartScreenButton : MonoBehaviour
{
    private Button button;
    private StartScreenManager startScreenManager;

    void Start()
    {
        button = GetComponent<Button>();
        startScreenManager = GameObject.Find("StartScreenManager").GetComponent<StartScreenManager>();
        button.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        Debug.Log(gameObject.name + " was clicked");
        startScreenManager.StartGame();
    }
}
