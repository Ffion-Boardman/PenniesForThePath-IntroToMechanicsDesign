using System;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class SpikeTrapFloor : MonoBehaviour
{
    [SerializeField] private int m_Damage;
    [SerializeField] private float m_DamageForce;
    [SerializeField] private int m_CoinLoss = 3;
    [SerializeField] private float m_UpwardsAngleFix = 1.5f;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.TryGetComponent<CharacterMovement>(out var characterMovement))
        {
            // Checking for player-invulnerability (if they're already stunned, don't hurt them)
            if (characterMovement.IsStunned)
            {
                Debug.Log(
                    "<b><color=grey>[GAME]</color> Player is currently <color=red>stunned</color> and <color=cyan>immune</color>.</b>");
                return;
            }
            
            Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");
            
            // Getting any contact-data we'll need
            ContactPoint2D m_Contact = collision.contacts[0];
            Rigidbody2D m_PlayerRB = characterMovement.GetComponent<Rigidbody2D>();

            // We want to get the contact-normal of the spike so we can reflect the direction of the force along it
            Vector2 m_SpikeNormal = m_Contact.normal;
            
            // Reflecting the player's velocity along the normal of the spike
            Vector2 m_Reflection = Vector2.Reflect(m_PlayerRB.velocity.normalized, m_SpikeNormal);
            Vector3 m_ContactPoint = collision.contacts[0].point;
            Debug.DrawLine(m_ContactPoint, m_Reflection, Color.red, 10f);
            
            // Compare that angle against the x-axis of the World
            
            m_Reflection.y = m_Reflection.y + m_UpwardsAngleFix;
            Debug.DrawLine(m_ContactPoint, m_Reflection, Color.yellow, 10f);
            
            // Now we can finally apply the impact-force on the player
            characterMovement.TakeDamageReaction(m_DamageForce, m_Reflection.normalized);
            
            // Now we apply any damage and coin loss the player will experience
            if (collision.transform.TryGetComponent<ScoreTracker>(out var scoreTracker))
            {
                // Apply coin loss
                scoreTracker.ApplyLoss(m_CoinLoss, this);
            }
            if (collision.transform.TryGetComponent<HealthComponent>(out var healthComponent))
            {
                // Apply health loss
                healthComponent.ApplyDamage(m_Damage, this);
            }
        }
    }
}