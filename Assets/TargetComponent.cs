using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : InteractiveComponent
{

    //private AudioSource m_audioSource;

    public AudioClip WoodHit;
    public ParticleSystem WoodHitParticles;

    private Vector3 m_startPosition;
    private Quaternion m_startRotation;

    Rigidbody2D m_rigidbody2d;
    AudioSource m_audioSource;


    void Start()
    {
        m_rigidbody2d = GetComponent<Rigidbody2D>();
        m_audioSource = FindObjectOfType<BallComponent>().GetComponent<AudioSource>();
        GameplayManager.OnGamePaused += DoPause;
        GameplayManager.OnGamePlaying += DoPlay;
        m_startPosition = transform.position;
        m_startRotation = transform.rotation;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            base.PlaySound(m_audioSource, WoodHit);
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

    public override void DoRestart()
    {
        base.DoRestart(m_rigidbody2d, m_startPosition, m_startRotation);
    }

    private void OnDestroy()
    {
        GameplayManager.OnGamePaused -= DoPause;
        GameplayManager.OnGamePlaying -= DoPlay;
    }
}
