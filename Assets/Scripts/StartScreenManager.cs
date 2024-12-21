using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    private UIManager uiManager;

    void Start()
    {
        PlayerPrefs.DeleteAll(); //When return to main menu, the high scores are reset. 
        uiManager = FindObjectOfType<UIManager>();
    }

    public void StartGame()
    {        
        SceneManager.LoadScene("BuzzyGrove");
    }    
}
