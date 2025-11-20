using System;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreUI : MonoBehaviour
{
    private ScoreTracker m_ScoreTracker;
    private Label m_ScoreLabel;

    [SerializeField] private UIDocument m_UIDocument;
    [SerializeField] private string m_LabelName = "ScoreCountLabel";

    private void Awake()
    {
        // Get the root of the UI document
        VisualElement root = m_UIDocument.rootVisualElement;
        
        // Find the Label element
        m_ScoreLabel = root.Q<Label>(m_LabelName);
        
        Debug.Assert(m_ScoreLabel != null, $"<b><color=red>[ERROR]</color><color=grey>[UI]</color> Could not find Label named '<color=yellow>{m_LabelName}</color>'</b>");
    }

    public void Initialize(ScoreTracker scoreTracker)
    {
        m_ScoreTracker = scoreTracker;
        
        // Make the UI associate to the coin-collection event
        m_ScoreTracker.OnCoinCollected += UpdateScoreDisplay;
        
        // Make the UI associate to the coin-lost event
        m_ScoreTracker.OnCoinLost += UpdateScoreDisplay;
        
        // On initialization, set the initial score display
        UpdateScoreDisplay(m_ScoreTracker.CurrentCoins, m_ScoreTracker.MaxCoins, 0);
    }

    private void OnDestroy()
    {
        if (m_ScoreTracker != null)
        {
            m_ScoreTracker.OnCoinCollected -= UpdateScoreDisplay;
            m_ScoreTracker.OnCoinLost -= UpdateScoreDisplay;
        }
    }

    private void UpdateScoreDisplay(int CurrentCoins, int MaxCoins, int ValueChanged)
    {
        // Format the score as a ratio
        m_ScoreLabel.text = $"{CurrentCoins}/{MaxCoins}";

        Debug.Log($"<b><color=grey>[UI]</color> Score display updated to: <color=yellow>{m_ScoreLabel.text}</color></b>");
    }
}
