using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int m_damage;
    [SerializeField] private float m_damageforce;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");
        if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
        {
            healthComponent.ApplyDamage(m_damage, this);
        }

        if (collision.transform.parent.TryGetComponent<Rigidbody2D>(out var rigidBody))
        {
            rigidBody.AddForce(new Vector2(0, m_damageforce), ForceMode2D.Impulse);
        }
    }
}