using System;
using UnityEngine;

public class ScoreTracker : MonoBehaviour
{
    public event Action<int, int, int> OnCoinCollected;
    public event Action<int, int, int> OnCoinLost;
    
    [SerializeField] private int m_MaxCoins;
    [SerializeField] private int m_CurrentCoins;
    
    void Start()
    {
        m_CurrentCoins = 0;
    }

    // These are for the score UI
    public int MaxCoins => m_MaxCoins;
    public int CurrentCoins => m_CurrentCoins;

    public void ApplyScore(int value, MonoBehaviour causer)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=yellow>Coin collected!</color></b>");
        
        // Play the right coin-sound
        SoundManager.PlaySound(SoundType.COIN, m_CurrentCoins, 1);
        
        // Increase the score, capping it at the maximum
        int m_PreviousCoins = m_CurrentCoins;
        m_CurrentCoins = Mathf.Min(m_CurrentCoins + value, m_MaxCoins);
        int m_ActualValueAdded = m_CurrentCoins - m_PreviousCoins;
        
        // Only do this if the score actually changed
        if (m_ActualValueAdded > 0)
        {
            OnCoinCollected.Invoke(m_CurrentCoins, m_MaxCoins, m_ActualValueAdded);
        }
        
        Debug.Log($"<b><color=grey>[GAME]</color> Current Score: <color=yellow>{m_CurrentCoins}/{m_MaxCoins}</color></b>");
    }

    public void ApplyLoss(int lossAmount, MonoBehaviour causer)
    {
        // Ensure the loss amount is a positive number so we don't accidentally ADD coins
        lossAmount = Mathf.Abs(lossAmount);
        
        // Figure out how many coins will actually be lost so we don't go into the negatives with the score
        int m_Change = Mathf.Min(m_CurrentCoins, lossAmount);

        if (m_Change > 0)
        {
            // Reduce the coins!
            m_CurrentCoins -= m_Change;
            
            Debug.Log($"<b><color=grey>[GAME]</color> Lost <color=red>{m_Change}</color> coins to <color=red>{causer.gameObject.name}</color></b>");
            
            // Invoke the event for the UI
            OnCoinLost?.Invoke(m_CurrentCoins, m_MaxCoins, -m_Change);
        }
    }
}
