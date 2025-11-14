using System;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<int, int, int> OnDamaged;
    public event Action<MonoBehaviour> OnDead;

    [SerializeField] private int m_MaxHealth;
    [SerializeField] private int m_CurrentHealth;
    
    void Start()
    {
        m_CurrentHealth = m_MaxHealth;
    }

    public void ApplyDamage(int damage, MonoBehaviour causer)
    {
        int change = Mathf.Min(m_CurrentHealth, damage);
        m_CurrentHealth -= change;
        
        OnDamaged?.Invoke(m_CurrentHealth, m_MaxHealth, damage);
        if (m_CurrentHealth == 0) { OnDead?.Invoke(causer); }
    }
}