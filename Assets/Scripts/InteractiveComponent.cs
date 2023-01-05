using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour, IRestartableObject
{
    public Vector3 m_startPosition;
    public Quaternion m_startRotation;

    public Rigidbody2D m_rigidbody2d;

    private void Awake()
    {
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;
        m_rigidbody2d = GetComponent<Rigidbody2D>();
    }
    public virtual void DoRestart()
    {
        transform.position = m_startPosition;
        transform.rotation = m_startRotation;

        m_rigidbody2d.velocity = Vector3.zero;
        m_rigidbody2d.angularVelocity = 0.0f;
        m_rigidbody2d.simulated = true;
    }

    public virtual void PlaySound(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.PlayOneShot(audioClip);
    }


    public void DoPlay()
    {
        m_rigidbody2d.simulated = true;
    }

    public void DoPause()
    {
        m_rigidbody2d.simulated = false;
    }
}
