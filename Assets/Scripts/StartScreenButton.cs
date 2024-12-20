using UnityEngine;
using UnityEngine.UI;

public class StartScreenButton : MonoBehaviour
{
    private Button button;
    private StartScreenManager startScreenManager;

    public enum ButtonAction { StartGame }
    public ButtonAction buttonAction;

    void Start()
    {
        button = GetComponent<Button>();
        startScreenManager = GameObject.Find("StartScreenManager").GetComponent<StartScreenManager>();

        if (buttonAction == ButtonAction.StartGame)
        {
            button.onClick.AddListener(startScreenManager.StartGame);
        }
    }
}
