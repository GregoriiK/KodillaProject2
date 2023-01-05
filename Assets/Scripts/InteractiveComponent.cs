using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveComponent : MonoBehaviour, IRestartableObject
{
    public virtual void DoRestart(){}
    public virtual void DoRestart(Rigidbody2D m_rigidbody2d, Vector3 m_startPosition, Quaternion m_startRotation)
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
}
