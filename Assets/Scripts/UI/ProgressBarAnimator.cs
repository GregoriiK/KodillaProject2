using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBarAnimator : MonoBehaviour
{
    public float LoadingTime = 5;
    public float imageFillAmount = 0;
    Slider m_slider;

    private void Start()
    {
        m_slider = GetComponent<Slider>();
    }

    void Update()
    {
        imageFillAmount = Mathf.Repeat(Time.time / LoadingTime, 1) ;
        m_slider.value = imageFillAmount ;
    }
}
