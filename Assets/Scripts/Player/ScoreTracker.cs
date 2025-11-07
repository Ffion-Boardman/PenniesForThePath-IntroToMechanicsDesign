using System;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public event Action<float, float, float> OnCoinCollected;
    
    [SerializeField] private float m_MaxCoins;
    [SerializeField] private float m_CurrentCoins;
    
    void Start()
    {
        m_CurrentCoins = 0;
    }

    public void ApplyScore(float value, MonoBehaviour causer)
    {
        
    }
}
