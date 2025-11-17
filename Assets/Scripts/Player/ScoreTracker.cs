using System;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public event Action<int, int, int> OnCoinCollected;
    //public event Action<int, int, int> OnDamaged;
    
    [SerializeField] private int m_MaxCoins;
    [SerializeField] private int m_CurrentCoins;
    
    void Start()
    {
        m_CurrentCoins = 0;
    }

    public void ApplyScore(float value, MonoBehaviour causer)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=yellow>Coin collected!</color></b>");
        
        // Play the right coin-sound
        SoundManager.PlaySound(SoundType.COIN, m_CurrentCoins, 1);
        
        // Increase the score
        if (m_CurrentCoins < m_MaxCoins)
        {
            m_CurrentCoins++;
        }
        
        Debug.Log($"<b><color=grey>[GAME]</color> Current Score: <color=yellow>{m_CurrentCoins}/{m_MaxCoins}</color></b>");
    }
}
