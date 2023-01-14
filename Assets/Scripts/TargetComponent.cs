using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetComponent : InteractiveComponent
{

    public GameSettingsDatabase GameDatabase;
    AudioSource m_audioSource;

    void Start()
    {
        m_audioSource = FindObjectOfType<BallComponent>().GetComponent<AudioSource>();

        GameplayManager.OnGamePaused += DoPause;
        GameplayManager.OnGamePlaying += DoPlay;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.gameObject.layer == LayerMask.NameToLayer("Ball"))
        {
            PlaySound(m_audioSource, GameDatabase.WoodHit);
            Instantiate(GameDatabase.WoodHitParticles, collision.GetContact(0).point, Quaternion.identity);
            GameplayManager.Instance.Points += 1;
            SaveManager.Instance.SaveData.m_lifetimeHits += 1;
            //GameplayManager.Instance.LifetimeHits += 1;
        }
    }

    private void OnDestroy()
    {
        GameplayManager.OnGamePaused -= DoPause;
        GameplayManager.OnGamePlaying -= DoPlay;
    }
}
