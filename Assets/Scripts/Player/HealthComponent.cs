using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<float, float, float> OnDamaged;
    public event Action<MonoBehaviour> onDead;

    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_CurrentHealth;
    
    void Start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void ApplyDamage(float damage, MonoBehaviour causer)
    {
        float change = Mathf.Min(m_CurrentHealth, damage);
        m_CurrentHealth -= change;
        
        OnDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, damage);
        if (m_CurrentHealth == 0) { onDead?.Invoke(causer); }
    }
}