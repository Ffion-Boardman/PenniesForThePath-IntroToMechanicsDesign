using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private HealthComponent m_HealthComponent;

    private void Awake()
    {
        m_HealthComponent = GetComponent<HealthComponent>();
        Debug.Assert(m_HealthComponent != null);
    }

    private void Start()
    {
        m_HealthComponent.OnDamaged += Handle_OnHealthDamage;
        m_HealthComponent.onDead += Handle_OnPlayerDies;
    }

    private void OnDestroy()
    {
        m_HealthComponent.OnDamaged -= Handle_OnHealthDamage;
        m_HealthComponent.onDead -= Handle_OnPlayerDies;
    }

    private void Handle_OnHealthDamage(float currentHealth, float maxHealth, float inboundDamage)
    {
        Debug.Log($"Current Health is: {currentHealth} out of: {maxHealth} and Damage received was: {inboundDamage}");
    }

    private void Handle_OnPlayerDies(MonoBehaviour causer)
    {
        Debug.Log($"Our player died, due to {causer.gameObject.name}");
    }
}