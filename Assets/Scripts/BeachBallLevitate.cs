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

    void Start()
    {
        m_startPosition = transform.position;
    }

    void Update()
    {
        m_curYPos = Mathf.PingPong(Time.time, Amplitude) - Amplitude * 0.5f;
        transform.position = new Vector3(m_startPosition.x,
                                         m_startPosition.y + m_curYPos,
                                         m_startPosition.z);

        m_curZRot += Time.deltaTime * RotationSpeed;
        transform.rotation = Quaternion.Euler(0, 0, m_curZRot);
    }
}
