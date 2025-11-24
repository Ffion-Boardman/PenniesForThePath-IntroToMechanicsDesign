using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    [Header("UI Components")]
    [SerializeField] private HealthUI m_HealthUI;
    [SerializeField] private ScoreUI m_ScoreUI;
    [SerializeField] private HealthComponent m_HealthComponent;
    [SerializeField] private ScoreTracker m_ScoreTracker;
    
    [Header("Player Components")]
    [SerializeField] private Animator m_Animator;
    [SerializeField] private CharacterMovement m_CharacterMovement;
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private BoxCollider2D m_PlayerCollider;
    [SerializeField] private CinemachineCamera m_CinemachineCamera;
    private Coroutine m_CDeathRoutine;

    private void Awake() 
    {
        // Getting the UI
        m_HealthComponent = GetComponent<HealthComponent>();
        m_ScoreTracker = GetComponent<ScoreTracker>();
        
        Debug.Assert(m_HealthComponent != null);
        Debug.Assert(m_ScoreTracker != null);
        
        Debug.Assert(m_HealthUI != null, "<b><color=red>[ERROR]</color><color=grey>[UI]</color> HealthUI reference is missing in PlayerController.</b>");
        Debug.Assert(m_ScoreUI != null, "<b><color=red>[ERROR]</color><color=grey>[UI]</color> ScoreUI reference is missing in PlayerController.</b>");
    }

    private void Start()
    {
        // Initializing health and Score UI
        if (m_HealthUI != null) { m_HealthUI.Initialize(m_HealthComponent); }
        if (m_ScoreUI != null) { m_ScoreUI.Initialize(m_ScoreTracker); }
        
        // Damage and Death events
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
        
        // Play a hurt-sound
        SoundManager.PlayRandomSound(SoundType.HURT, 0.5f);
    }

    private void Handle_OnPlayerDies(MonoBehaviour causer)
    {
        Debug.Log($"<b><color=grey>[GAME]</color> Player has <color=red>died</color> due to: <color=yellow>{causer.gameObject.name}</color></b>");
        
        // Start the death-routine
        if (m_CDeathRoutine != null)
        {
            StopCoroutine(m_CDeathRoutine);
        }
        m_CDeathRoutine = StartCoroutine(C_DeathRoutine());
    }

    IEnumerator C_DeathRoutine()
    {
        // Death-sprite
        m_Animator.SetBool("isDead", true);
        
        // Play the death-sound
        SoundManager.PlayRandomSound(SoundType.DEATH, 1);
        
        // Stop the camera from following the player
        m_CinemachineCamera.Follow = null;
        
        // Stop the player from colliding with anything so they just fall off the screen
        m_PlayerCollider.enabled = false;
        
        // Stop the player from being able to move around anymore
        m_CharacterMovement.enabled = false;

        // Wait for a few moments to let the player acknowledge they've died
        yield return new WaitForSeconds(2f);
        
        // We now go to the 'Death' scene
        SceneManager.LoadScene("Death");
    }
}