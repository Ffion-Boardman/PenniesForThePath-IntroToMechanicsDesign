using System.Collections;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private ParticleSystem m_CollectionParticles;
    private Coroutine m_CollectionCoroutine;

    private void Start()
    {
        m_SpriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.parent.TryGetComponent<ScoreTracker>(out var scoreTracker))
        {
            // Update the score
            scoreTracker.ApplyScore(1, this);
            // Play VFX
            m_CollectionParticles.Play();
            // Start timer
            StartCoroutine(C_CollectionCoroutine());
        }
    }

    IEnumerator C_CollectionCoroutine()
    {
        // Hide the sprite whilst the particles play
        m_SpriteRenderer.enabled = false;
        // Wait until the particles are done
        yield return new WaitForSeconds(2);
        // Now get rid of the game object
        Destroy(gameObject);
    }
}
