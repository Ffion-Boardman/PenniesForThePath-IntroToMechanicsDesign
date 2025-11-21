using System;
using UnityEngine;

public class SpikeTrap : MonoBehaviour
{
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_DamageForce;
    [SerializeField] private int m_CoinLoss = 3;
    [SerializeField] private float m_DirectionalTolerance = 0.2f; // This is to tell if the hit is mostly horizontal or vertical
    private CompositeCollider2D m_SpikeCollider;
    [SerializeField] private bool m_SpikeFloor;
    [SerializeField] private bool m_SpikeWall;

    private void Start()
    {
        m_SpikeCollider = GetComponent<CompositeCollider2D>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 test = other.contacts[0].normal;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");
        
        // Initial check for if we CAN hurt the player (we can't if they're already stunned and immune)
        if (collision.transform.parent.TryGetComponent<CharacterMovement>(out var characterMovement))
        {
            if (characterMovement.IsStunned)
            {
                Debug.Log("<b><color=grey>[GAME]</color> Player is currently <color=red>stunned</color> and <color=cyan>immune</color>.</b>");
                return;
            }
            
            // Apply health loss
            if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
            {
                healthComponent.ApplyDamage(m_Damage, this);
            }
            
            // Apply coin loss
            if (collision.transform.parent.TryGetComponent<ScoreTracker>(out var scoreTracker))
            {
                scoreTracker.ApplyLoss(m_CoinLoss, this);
            }
            
            // NEW
            Vector2 contactPoint = collision.ClosestPoint(characterMovement.transform.position);
            Vector2 characterVelocity = characterMovement.GetVelocity();
            
            //// Calculate knockback-direction
            //Vector2 spikePosition = transform.position;
            //Vector2 playerPosition = characterMovement.transform.position;
            
            //// Now calculate the general-ish direction vector from the spike to the player
            //Vector2 generalDirection = (playerPosition - spikePosition).normalized;
            //Vector2 finalKnockbackDirection;
            
            
            // NEW
            Vector2 reflectionDirection = Vector2.Reflect(characterVelocity, new Vector2(-1,0));
            
            //// Is the hit primarily horizontal, vertical, or (in a rare instance; diagonal?)
            //if (Mathf.Abs(generalDirection.x) > Mathf.Abs(generalDirection.y) + m_DirectionalTolerance) // Wall hit
            //{
            //    // Normalizing just to play it safe
            //    finalKnockbackDirection = new Vector2(Mathf.Sign(generalDirection.x), 0f).normalized; 
            //    Debug.Log("<b><color=grey>[SPIKE]</color> Wall Hit: Applied <color=cyan>Horizontal</color> Knockback.</b>");
            //}
            //else if (Mathf.Abs(generalDirection.y) > Mathf.Abs(generalDirection.x) + m_DirectionalTolerance) // Floor hit
            //{
            //    // Normalizing just to play it safe
            //    finalKnockbackDirection = new Vector2(0f, Mathf.Sign(generalDirection.y)).normalized; 
            //    Debug.Log("<b><color=grey>[SPIKE]</color> Floor Hit: Applied <color=cyan>Vertical</color> Knockback.</b>");
            //}
            //else // Some sort of diagonal hit
            //{
            //    finalKnockbackDirection = generalDirection;
            //    Debug.Log("<b><color=grey>[SPIKE]</color> Diagonal Hit: Applied <color=cyan>General</color> Knockback.</b>");
            //}
            
            // Now, we finally apply the knockback, stun, and invulnerability!
            characterMovement.TakeDamageReaction(m_DamageForce, reflectionDirection);
        }
    }
}