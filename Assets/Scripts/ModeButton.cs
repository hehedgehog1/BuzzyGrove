using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ModeButton : MonoBehaviour
{
    private Button button;
    private GameManager gameManager;

    public int mode;

    void Start()
    {
        button = GetComponent<Button>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        button.onClick.AddListener(SetGameMode);
    }

    void SetGameMode()
    {
        Debug.Log(gameObject.name + " was clicked");
        StartGame(mode);
    }
}
