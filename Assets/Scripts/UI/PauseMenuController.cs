using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public Button ResumeButton;
    public Button RestartButton;
    public Button QuitButton;
    public Button YesButton;
    public Button NoButton;
    public Button SaveButton;
    public Button LoadButton;
    public GameObject Panel;
    public GameObject QuestionPopup;

    Canvas m_HUD;
    MainMenuController m_mainMenu;

    void Start()
    {
        QuestionPopup.SetActive(false);
        ResumeButton.onClick.AddListener(delegate { OnResume(); });
        QuitButton.onClick.AddListener(delegate { OnQuit(); });
        RestartButton.onClick.AddListener(delegate { OnRestart(); });
        YesButton.onClick.AddListener(delegate { Confirm(); });
        NoButton.onClick.AddListener(delegate { Decline(); });
        SaveButton.onClick.AddListener(delegate { Save(); });
        LoadButton.onClick.AddListener(delegate { Load(); });

        m_HUD = FindObjectOfType<HUDController>().GetComponent<Canvas>();
        m_mainMenu = FindObjectOfType<MainMenuController>();

        GameplayManager.OnGamePaused += OnPause;
        GameplayManager.OnGamePlaying += OnPlay;

        OnPause();
    }

    public void SetPanelVisible(bool visible)
    {
        Panel.SetActive(visible);
    }

    private void OnPause()
    {
        if (m_mainMenu.MainPanel.activeInHierarchy)
        {
            SetPanelVisible(false);
            m_HUD.enabled = false;
        }
        else
        {
            SetPanelVisible(true);
            m_HUD.enabled = false;
        }
    }

    private void OnPlay()
    {
        SetPanelVisible(false);
        m_HUD.enabled = true;
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
        ToggleButtonsInteractable();
        QuestionPopup.SetActive(true);
    }

    private void ToggleButtonsInteractable()
    {
        ResumeButton.interactable = !ResumeButton.interactable;
        RestartButton.interactable = !RestartButton.interactable;
        QuitButton.interactable = !QuitButton.interactable;
    }

    private void Confirm()
    {
        SaveManager.Instance.SaveSettings();
        //GameplayManager.Instance.SaveHitcount();
        //Application.Quit();
        ToggleButtonsInteractable();
        QuestionPopup.SetActive(false);
        OnRestart();
        m_mainMenu.MainPanel.SetActive(true);
        GameplayManager.Instance.PlayPause();
    }

    private void Decline()
    {
        ToggleButtonsInteractable();
        QuestionPopup.SetActive(false);
    }

    private void Save()
    {
        SaveManager.Instance.SaveSettings();
    }

    private void Load()
    {
        SaveManager.Instance.LoadSettings();
    }

}
