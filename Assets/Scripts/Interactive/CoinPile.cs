using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinPile : MonoBehaviour
{
    [SerializeField] private ParticleSystem m_VictoryParticles;
    [SerializeField] private float m_VictoryRelishTime;
    private CircleCollider2D m_CircleCollider2D;
    private Coroutine m_CCoinPile;

    private void Awake()
    {
        m_CircleCollider2D = GetComponent<CircleCollider2D>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.transform.parent.TryGetComponent<ScoreTracker>(out var scoreTracker))
        {
            StartCoroutine(C_Victory());
        }
    }
    
    IEnumerator C_Victory()
    {
        // Play the victory particles
        m_VictoryParticles.Play();
        
        // Disable the collider so the player can't trigger it again
        m_CircleCollider2D.enabled = false;
        
        // Play the victory sound
        SoundManager.PlaySound(SoundType.UI, 0, 1);
        
        // Wait for a moment
        yield return new WaitForSeconds(m_VictoryRelishTime);
        
        // Load the victory scene!
        SceneManager.LoadScene("Victory");
    }
}
