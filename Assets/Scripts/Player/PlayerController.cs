using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HealthUI m_HealthUI;
    [SerializeField] private ScoreUI m_ScoreUI;
    [SerializeField] private HealthComponent m_HealthComponent;
    [SerializeField] private ScoreTracker m_ScoreTracker;
    
    private SpriteRenderer m_SpriteRenderer;
    private Sprite m_SpriteDeath;
    private Sprite m_SpriteHurt;

    private void Awake()
    {
        m_HealthComponent = GetComponent<HealthComponent>();
        m_ScoreTracker = GetComponent<ScoreTracker>();
        
        Debug.Assert(m_HealthComponent != null);
        Debug.Assert(m_ScoreTracker != null);
        
        Debug.Assert(m_HealthUI != null, "<b><color=red>[ERROR]</color><color=grey>[UI]</color> HealthUI reference is missing in PlayerController.</b>");
        Debug.Assert(m_ScoreUI != null, "<b><color=red>[ERROR]</color><color=grey>[UI]</color> ScoreUI reference is missing in PlayerController.</b>");
    }

    private void Start()
    {
        m_SpriteRenderer  = GetComponent<SpriteRenderer>();

        if (m_HealthUI != null)
        {
            m_HealthUI.Initialize(m_HealthComponent);
        }

        if (m_ScoreUI != null)
        {
            m_ScoreUI.Initialize(m_ScoreTracker);
        }
        
        m_HealthComponent.OnDamaged += Handle_OnHealthDamage;
        m_HealthComponent.OnDead += Handle_OnPlayerDies;
    }

    private void OnDestroy()
    {
        m_HealthComponent.OnDamaged -= Handle_OnHealthDamage;
        m_HealthComponent.OnDead -= Handle_OnPlayerDies;
    }

    private void Handle_OnHealthDamage(int currentHealth, int maxHealth, int inboundDamage)
    {
        Debug.Log($"<b><color=grey>[GAME]</color> Current Health is: <color=yellow>{currentHealth}</color>/<color=green>{maxHealth}</color> and Damage received was <color=red>{inboundDamage}</color></b>");
    }

    private void Handle_OnPlayerDies(MonoBehaviour causer)
    {
        Debug.Log($"<b><color=grey>[GAME]</color> Player has <color=red>died</color> due to: <color=yellow>{causer.gameObject.name}</color></b>");
    }
}