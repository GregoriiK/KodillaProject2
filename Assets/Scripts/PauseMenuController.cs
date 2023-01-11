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
    public GameObject Panel;
    public GameObject QuestionPopup;

    void Start()
    {
        QuestionPopup.SetActive(false);
        ResumeButton.onClick.AddListener(delegate { OnResume(); });
        QuitButton.onClick.AddListener(delegate { OnQuit(); });
        RestartButton.onClick.AddListener(delegate { OnRestart(); });
        YesButton.onClick.AddListener(delegate { Confirm(); });
        NoButton.onClick.AddListener(delegate { Decline(); });

        SetPanelVisible(false);

        GameplayManager.OnGamePaused += OnPause;
    }

    public void SetPanelVisible(bool visible)
    {
        Panel.SetActive(visible);
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
        GameplayManager.Instance.SaveHitcount();
        Application.Quit();
    }

    private void Decline()
    {
        ToggleButtonsInteractable();
        QuestionPopup.SetActive(false);
    }

}
