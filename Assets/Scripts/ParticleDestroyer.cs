using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    private ParticleSystem m_particles;

    private void Start()
    {
        m_particles = GetComponent<ParticleSystem>();
    }
    void Update()
    {
        if (!m_particles.IsAlive())
        {
            Destroy(gameObject);
        }
    }
}
