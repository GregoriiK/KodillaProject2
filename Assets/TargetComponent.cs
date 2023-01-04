using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : MonoBehaviour, IRestartableObject
{

    private AudioSource m_audioSource;
    private Vector3 m_startPosition;
    private Quaternion m_startRotation;

    public AudioClip WoodHit;
    public ParticleSystem WoodHitParticles;

    Rigidbody2D m_rigidbody2d;

    void Start()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_audioSource = GetComponent<AudioSource>();
        GameplayManager.OnGamePaused += DoPause;
        GameplayManager.OnGamePlaying += DoPlay;
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            m_audioSource.PlayOneShot(WoodHit);
            Instantiate(WoodHitParticles, collision.GetContact(0).point, Quaternion.identity);
        }
    }

    private void DoPlay()
    {
        m_rigidbody2d.simulated = true;
    }

    private void DoPause()
    {
        m_rigidbody2d.simulated = false;
    }

    public void DoRestart()
    {
        transform.position = m_startPosition;
        transform.rotation = m_startRotation;

        m_rigidbody2d.velocity = Vector3.zero;
        m_rigidbody2d.angularVelocity = 0.0f;
        m_rigidbody2d.simulated = true;
    }

    private void OnDestroy()
    {
        GameplayManager.OnGamePaused -= DoPause;
        GameplayManager.OnGamePlaying -= DoPlay;
    }
}
