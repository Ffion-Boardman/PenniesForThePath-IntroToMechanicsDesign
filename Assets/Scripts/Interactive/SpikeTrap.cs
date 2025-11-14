using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int m_damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");
        if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
        {
            healthComponent.ApplyDamage(m_damage, this);
        }
    }
}