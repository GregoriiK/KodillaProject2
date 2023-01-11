using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeachBallLevitate : MonoBehaviour
{
    private Vector3 m_startPosition;

    private float m_curYPos = 0.0f;
    private float m_curZRot = 0.0f;

    public float Amplitude = 1.0f;
    public float RotationSpeed = 50;
    public float holdTime = 1f;

    void Start()
    {
        m_startPosition = transform.position;
        StartCoroutine(LevitationHold());
    }

    private void Update()
    {        
            m_curZRot += Time.deltaTime * RotationSpeed;
            transform.rotation = Quaternion.Euler(0, 0, m_curZRot);
    }

    private IEnumerator LevitationHold()
    {
        float previousPos;
        float lastPosition = 0f;
        float timer = 0f;

        while (true)
        {
            timer += Time.deltaTime;
            previousPos = lastPosition;
            lastPosition = m_curYPos;
            m_curYPos = Mathf.PingPong(timer, Amplitude); // - Amplitude * 0.5f;
            transform.position = new Vector3(m_startPosition.x,
                                             m_startPosition.y + m_curYPos,
                                             m_startPosition.z);

            if (lastPosition - previousPos < 0 && m_curYPos - lastPosition > 0)
            {
                yield return new WaitForSeconds(holdTime);
            }
            else
            {
                yield return null;
            }
        }        
    }
}
