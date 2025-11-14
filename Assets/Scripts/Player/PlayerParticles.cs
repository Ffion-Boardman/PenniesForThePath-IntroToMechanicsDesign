using UnityEngine;

public class PlayerParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_JumpParticles;
    
    public void PlaySystem()
    {
        m_JumpParticles.Play();
    }
}
