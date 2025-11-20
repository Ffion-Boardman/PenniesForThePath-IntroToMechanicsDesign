using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int m_damage;
    [SerializeField] private float m_damageforce;
    [SerializeField] private int m_CoinLoss = 3;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");
        
        // Apply damage
        if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
        {
            healthComponent.ApplyDamage(m_damage, this);
        }

        // Apply coin-loss
        if (collision.transform.parent.TryGetComponent<ScoreTracker>(out var scoreTracker))
        {
            scoreTracker.ApplyLoss(m_CoinLoss, this);
        }
        
        // Apply knockback force
        if (collision.transform.parent.TryGetComponent<Rigidbody2D>(out var rigidBody))
        {
            rigidBody.AddForce(new Vector2(0, m_damageforce), ForceMode2D.Impulse);
        }
    }
}