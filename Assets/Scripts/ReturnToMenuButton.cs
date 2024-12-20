using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ReturnToMenuButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ReturnToMainMenu);
    }

    void ReturnToMainMenu()
    {
        SceneManager.LoadScene("StartScreen");
    }
}
