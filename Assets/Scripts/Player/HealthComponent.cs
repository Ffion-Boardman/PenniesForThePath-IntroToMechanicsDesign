using System;
using Unity.VisualScripting;
using UnityEngine;

public class HealthComponent : MonoBehaviour
{
    public event Action<int, int, int> OnDamaged;
    public event Action<MonoBehaviour> OnDead;

    [SerializeField] private int m_MaxHealth;
    [SerializeField] private int m_CurrentHealth;
    
    // These are for the health UI
    public int MaxHealth => m_MaxHealth;
    public int CurrentHealth => m_CurrentHealth;
    
    void Awake()
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