using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Button ResumeButton;
    public Button RestartButton;
    public Button QuitButton;
    public GameObject Panel;
    //HUDController m_HUD;

    void Start()
    {
        //m_HUD = FindObjectOfType<HUDController>();

        ResumeButton.onClick.AddListener(delegate { OnResume(); });
        QuitButton.onClick.AddListener(delegate { OnQuit(); });
        RestartButton.onClick.AddListener(delegate { OnRestart(); });

        SetPanelVisible(false);

        GameplayManager.OnGamePaused += OnPause;
    }

    public void SetPanelVisible(bool visible)
    {
        Panel.SetActive(visible);
        //m_HUD.gameObject.SetActive(!visible);
    }

    private void OnPause()
    {
        SetPanelVisible(true);
    }

    private void OnResume()
    {
        GameplayManager.Instance.GameState = GameplayManager.EGameState.Playing;
        SetPanelVisible(false);
    }

    private void OnRestart()
    {
        GameplayManager.Instance.Restart();
        SetPanelVisible(false);
    }

    private void OnQuit()
    {
        Application.Quit();
    }

}
