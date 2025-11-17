using System.Collections.Generic;
using UnityEngine;

public class CoinParticleCollision : MonoBehaviour
{
    ParticleSystem ps;
    
    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();

    private void Start()
    {
        ps = GetComponent<ParticleSystem>();
    }

    private void OnParticleTrigger(Collider other)
    {
        int triggeredParticles = ps.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        for (int i = 0; i < triggeredParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            particles[i] = p;
        }
        
        ps.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
    }
}
