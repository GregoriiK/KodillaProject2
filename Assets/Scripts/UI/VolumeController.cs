using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public Button VolumeUp;
    public Button VolumeDown;
    public Image Bar;

    private void Start()
    {
        VolumeUp.onClick.AddListener(delegate { OnChangeVolume(true); });
        VolumeDown.onClick.AddListener(delegate { OnChangeVolume(false); });
    }

    private void UpdateBar()
    {
        Bar.fillAmount = AudioListener.volume;
    }

    private void OnEnable()
    {
        UpdateBar();
    }

    private void OnChangeVolume(bool bUp)
    {
        float newValue = AudioListener.volume;

        if (bUp)
            newValue += 0.1f;
        else
            newValue -= 0.1f;

        newValue = Mathf.Clamp01(newValue);
        AudioListener.volume = newValue;
        UpdateBar();
    }
}