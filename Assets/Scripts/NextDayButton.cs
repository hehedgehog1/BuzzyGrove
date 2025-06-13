using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextDayButton : MonoBehaviour
{
    private Button button;

    void Start()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ContinueGame);
    }

    void ContinueGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
