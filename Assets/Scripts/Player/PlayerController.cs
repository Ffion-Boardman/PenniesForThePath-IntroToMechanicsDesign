using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HealthComponent m_HealthComponent;
    private SpriteRenderer m_SpriteRenderer;
    private Sprite m_SpriteDeath;
    private Sprite m_SpriteHurt;

    private void Awake()
    {
        m_HealthComponent = GetComponent<HealthComponent>();
        Debug.Assert(m_HealthComponent != null);
    }

    private void Start()
    {
        m_SpriteRenderer  = GetComponent<SpriteRenderer>();
        
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