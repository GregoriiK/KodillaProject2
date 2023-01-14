using UnityEngine;
using UnityEngine.UI;

public class MainMenuController : MonoBehaviour
{
    public Button PlayButton;
    public Button OptionsButton;
    public Button QuitButton;

    public GameObject MainPanel;
    public GameObject MainMenuPanel;
    public GameObject OptionsPanel;

    void Start()
    {
        PlayButton.onClick.AddListener(delegate { OnPlay(); });
        OptionsButton.onClick.AddListener(delegate { ShowOptions(true); });
        QuitButton.onClick.AddListener(delegate { OnQuit(); });

        SetPanelVisible(true);
        OptionsPanel.SetActive(false);
        MainMenuPanel.SetActive(true);
    }

    public void SetPanelVisible(bool visible)
    {
        MainPanel.SetActive(visible);
    }

    private void OnPlay()
    {
        SetPanelVisible(false);
        FindObjectOfType<GameplayManager>().GameState = GameplayManager.EGameState.Playing;
    }

    public void ShowOptions(bool bShow)
    {
        OptionsPanel.SetActive(bShow);
        MainMenuPanel.SetActive(!bShow);
    }

    private void OnQuit()
    {
        Debug.Log("Closing app");
        Application.Quit();
    }

}