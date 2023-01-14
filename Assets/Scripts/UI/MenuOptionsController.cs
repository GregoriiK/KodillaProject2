using UnityEngine;
using UnityEngine.UI;

public class MenuOptionsController : MonoBehaviour
{
    public Button CancelButton;
    public Button AcceptButton;

    public MainMenuController MainMenu;

    private float m_initialVolume = 0.0f;

    void Start()
    {
        CancelButton.onClick.AddListener(delegate { OnCancel(); });
        AcceptButton.onClick.AddListener(delegate { OnAccept(); });

    }

    private void OnEnable()
    {
        m_initialVolume = AudioListener.volume;
    }

    private void OnCancel()
    {
        AudioListener.volume = m_initialVolume;
        MainMenu.ShowOptions(false);
    }

    private void OnAccept()
    {
        SaveManager.Instance.SaveData.m_masterVolume = AudioListener.volume;
        SaveManager.Instance.SaveSettings();
        MainMenu.ShowOptions(false);
    }
}