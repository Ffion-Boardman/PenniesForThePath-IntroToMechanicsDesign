using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] public ParticleSystem m_JumpParticles;
    [SerializeField] public ParticleSystem m_RunParticles;
    
    public void PlaySystem(ParticleSystem system)
    {
        system.Play();
    }
    
    public void StopSystem(ParticleSystem system)
    {
        system.Stop();
    }
}
