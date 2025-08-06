using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class StartSceneCode : MonoBehaviour
{
    [Header("버튼 지정")]
    public Button startButton;
    public Button exitButton;
    public Button helpButton;
    public Button exitHelpPanelButton;

    [Header("UI")]
    public GameObject helpPanel;


    void Start()
    {
        startButton.onClick.AddListener(StartButton);
        exitButton.onClick.AddListener(ExitButton);
        helpButton.onClick.AddListener(HelpButton);
        exitHelpPanelButton.onClick.AddListener(ExitHelpPanelButton);

        helpPanel.SetActive(false);
    }


    void StartButton()
    {
        SceneManager.LoadScene("PlayScene");
    }
    void ExitButton()
    {
        Application.Quit();
    }
    void HelpButton()
    {
        helpPanel.SetActive(true);
    }
    void ExitHelpPanelButton()
    {
        helpPanel.SetActive(false);
    }
}
