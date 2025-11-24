using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Void : MonoBehaviour
{
    [SerializeField] private int m_Damage;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");
        
        // Kill the player
        if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
        {
            healthComponent.ApplyDamage(m_Damage, this);
        }
    }
}