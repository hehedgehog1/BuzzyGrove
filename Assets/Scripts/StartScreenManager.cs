using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenManager : MonoBehaviour
{
    public GameObject highScorePanel;

    public void StartGame()
    {        
        SceneManager.LoadScene("BuzzyGrove");
    }

    public void ShowHighScores()
    {
        highScorePanel.SetActive(true);
    }

    public void HideHighScores()
    {
        highScorePanel.SetActive(false);
    }     
}
