using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Kodilla.Module8.Scripts
{
    public class FadingImage : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Image image;
        [SerializeField] private float fadeTime = 0.5f;

        private bool isOn;
        private bool isChanging;

        public float FadeTime
        {
            get => fadeTime; set => fadeTime = value;
        }

        private void Start()
        {
            button.onClick.AddListener(DoFade);
        }

        public void DoFade()
        {
            if (!isChanging)
            {
                StartCoroutine(Fade());
            }
        }

        private IEnumerator Fade()
        {
            isChanging = true;
            float finalState = isOn ? 0 : 1f;
            float time = 0f;
            float startAlpha = image.color.a;
            Color imageColor = image.color;
            while (time < fadeTime)
            {
                imageColor.a = Mathf.Lerp(startAlpha, finalState, time / fadeTime);
                image.color = imageColor;
                time += Time.deltaTime;
                yield return null;
            }
            isOn = !isOn;
            isChanging = false;
        }
    }
}
