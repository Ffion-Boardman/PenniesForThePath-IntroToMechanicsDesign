using System;
using System.Collections;
using Unity.Cinemachine;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region Values
    
    [Header("External Variables")]
    [SerializeField] GroundCheck m_GroundCheck;
    [SerializeField] private Animator m_Animator;
    [SerializeField] private PlayerParticles m_PlayerParticles;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;
    [SerializeField] private CinemachineCamera m_Camera;
    private CinemachineBasicMultiChannelPerlin m_CMBMCP;
    [SerializeField] [Range(1, 3)] private float m_AmplitudeGain;
    
    [Header("Basic Movement")]
    [SerializeField] private float m_MoveSpeed;
    [SerializeField] private float m_MoveForceMultiplier;
    [SerializeField] private float m_MoveForceCap;
	[SerializeField] private float m_JumpStrength;
    
    private Rigidbody2D m_RB;
    
    // Movement
    private float m_InMove;
    private PlayerControls m_ActionMap;
    // Movement Update
    private Coroutine m_CMove;
    private bool m_IsMoveActive;
    
    // Sprite Flipping (for the animator)
    private float m_OriginalScaleX;
    
    // Jump Update
    private Coroutine m_CJump;
    private bool m_IsJumpActive = false; 
    
    /// This event is called for the sake of the Collider-Controller script
    public event Action<bool> m_PlayerJumped;
    
    [Header("Movement Sounds")]
    [SerializeField] [Range(0, 1)] public float m_JumpVolume;
    [SerializeField] [Range(0, 1)] public float m_FootstepVolume;
    [SerializeField] [Range(0, 1)] public float m_LandVolume;

    [Header("Forgiveness Mechanics")]
    [SerializeField] [Range(0, 0.5f)] private float m_CoyoteTimeThreshold = 0.4f;
    private Coroutine m_CCoyoteTime;
    private bool m_CanCoyoteJump = false;
    
    // Jump Buffer
    [SerializeField] [Range(0, 0.5f)] private float m_JumpBufferThreshold = 0.4f;
    private Coroutine m_CJumpBuffer;
    private bool m_CanBufferJump = false;
    
    // Variable Jump Height
    private float m_VariableJumpHeightDamper = 2.0f;

    [Header("Stun & Damage")] 
    [SerializeField] private float m_StunDuration = 1.5f;
    [SerializeField] private float m_FlashInterval = 0.1f;
    [SerializeField] public ParticleSystem m_DamageParticles;
    
    private Coroutine m_CStun;
    private bool m_IsStunned = false;
    
    #endregion
    
    private void Awake()
    {
		m_RB = GetComponent<Rigidbody2D>();
        m_OriginalScaleX = transform.localScale.x;
        m_CMBMCP = m_Camera.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    #region Stun and Knockback

    // A public flag to check if the player can be moved
    public bool IsStunned => m_IsStunned;
    
    public void TakeDamageReaction(float m_KnockbackForce, Vector2 m_KnockbackDirection) // Applies knockback force and activates the stun coroutine
    {
        // If the player is already currently invulnerable, ignore the damage
        if (m_IsStunned)
        {
            return;
        }
        
        // Apply the force of the spike onto the player
        m_RB.AddForce(m_KnockbackDirection * m_KnockbackForce, ForceMode2D.Impulse);
        
        // Play the player-hurt particles
        m_DamageParticles.Play();
        
        // Start the stun and invulnerability coroutine
        if (m_CStun != null)
        {
            StopCoroutine(m_CStun);
        }
        m_CStun = StartCoroutine(C_DamageStun(m_StunDuration));
    }

    IEnumerator C_DamageStun(float m_StunFlashDuration) // Handles the player being stunned, invulnerable, and the visible flash on the sprite during both
    {
        // Stunned-bool
        m_IsStunned = true;
        m_Animator.SetBool("isStunned", true);
        
        // Jump-bool
        m_IsJumpActive = true;
        
        // Start camera-shake
        m_CMBMCP.AmplitudeGain = m_AmplitudeGain;
        
        // Flash-timer
        float m_StunFlashTimer = 0f;
        
        // Loop for the player to flash-red
        while (m_StunFlashTimer < m_StunFlashDuration)
        {
            // Toggle flashing effect
            m_SpriteRenderer.color = new Color(1, 0.5f, 0.5f, 1);
            
            // Add to the timer
            m_StunFlashTimer += m_FlashInterval;
            
            yield return new WaitForSeconds(m_FlashInterval);
            
            // Toggle flashing effect
            m_SpriteRenderer.color = new Color(1, 1, 1, 1);
            
            // Add to the timer
            m_StunFlashTimer += m_FlashInterval;
            
            yield return new WaitForSeconds(m_FlashInterval);
        }
        
        // Jump-animation (for in case the player is no-longer stunned but is still in the air)
        m_Animator.SetBool("isJumping", !m_GroundCheck.m_IsGrounded);
        
        // Make sure sprite's colour is set back to normal
        m_SpriteRenderer.color = new Color(1, 1, 1, 1);
        
        // Stop the camera-shake
        m_CMBMCP.AmplitudeGain = 0f;
        
        // Stun and invulnerability are now over
        m_IsStunned = false;
        m_CStun = null;
    }
    
    #endregion
    
    #region Movement
    public void SetInMove(float m_Direction) // Takes the input and applies it to the player movement
    {
        m_InMove = m_Direction;
        
        if (m_InMove == 0)
        {
            m_IsMoveActive = false;
        }
        else
        {
            if (m_IsMoveActive) { return; }

            m_IsMoveActive = true;
            
            m_CMove = StartCoroutine(C_MoveUpdate());
        }
    }
    
    private void HandleSpriteFlipping() // Flips the sprite for the character-animator
    {
        if (m_InMove > 0) 
        {
            // Moving right
            transform.localScale = new Vector3(-m_OriginalScaleX, transform.localScale.y, transform.localScale.z);
        }
        else if (m_InMove < 0)
        {
            // Moving left
            transform.localScale = new Vector3(m_OriginalScaleX, transform.localScale.y, transform.localScale.z);
        }
    }

    IEnumerator C_MoveUpdate() // For when the player is moving
    {
        while (m_IsMoveActive)
        {
            yield return new WaitForFixedUpdate();

            if (!IsStunned)
            {
                m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
            }

            if (!m_IsJumpActive)
            {
                m_PlayerParticles.PlaySystem(m_PlayerParticles.m_RunParticles);
            }
        }
    }

    public void MoveCancelled() // When the player stops moving
    {
        m_RB.linearVelocityX = 0;
        
        m_PlayerParticles.StopSystem(m_PlayerParticles.m_RunParticles);
    }

    #endregion

    #region Jump
    
    private void ExecuteJump() // When the player ACTUALLY jumps
    {
        // Jump-force
        m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        
        // Jump-bool
        m_IsJumpActive = true;
        
        // Jump-event
        m_PlayerJumped?.Invoke(true);
        
        // Jump-sound
        SoundManager.PlayRandomSound(SoundType.JUMP, m_JumpVolume);
        
        // Jump-particles
        m_PlayerParticles.PlaySystem(m_PlayerParticles.m_JumpParticles);
        
        // Jump-update
        StartCoroutine(C_JumpUpdate());
    }
    
    public void JumpPerformed() // When the player ATTEMPTS to jump (presses space)
    {
        // If jump isn't already active, AND if we're grounded, OR we're within coyote-limits: jump immediately
        if ( (m_GroundCheck.m_IsGrounded || m_CanCoyoteJump))
        {
            if (m_CanCoyoteJump) { Debug.Log("<b><color=grey>[FORGIVENESS]</color> Coyote-Time Used!</b>"); }
            ExecuteJump();
        }
        // If we're not grounded OR within coyote-limits: start the jump-buffer timer
        else
        { 
            // If the Jump-Buffer is already in effect, stop it so it can be reset
            if (m_CJumpBuffer != null)
            {
                StopCoroutine(m_CJumpBuffer);
            }
            // Execute the Jump-Buffer Timer
            m_CJumpBuffer = StartCoroutine(C_JumpBuffer());
        }
    }
    
    public void JumpCancelled() // Allows for variable jump height
    {
        if (m_RB.linearVelocity.y > 0)
        {
            m_RB.linearVelocityY /= m_VariableJumpHeightDamper;
        }
    }
    
    IEnumerator C_JumpUpdate()
    {
        while (m_IsJumpActive)
        {
            // Jump-animation
            m_Animator.SetBool("isJumping", !m_GroundCheck.m_IsGrounded);
            yield return new WaitForFixedUpdate();
        }
    } // For when the player IS jumping

    IEnumerator C_CoyoteTime() // This is the TIMER
    {
        m_CanCoyoteJump = true;
        yield return new WaitForSeconds(m_CoyoteTimeThreshold);
        m_CanCoyoteJump = false;
        m_CCoyoteTime = null;
    }
    
    IEnumerator C_JumpBuffer() // This is the TIMER
    {
        m_CanBufferJump = true;
        yield return new WaitForSeconds(m_JumpBufferThreshold);
        m_CanBufferJump = false;
        m_CJumpBuffer = null;
    }
    
    void OnCollisionEnter2D(Collision2D collision) // For when the player lands
    {
        if (m_GroundCheck.m_IsGrounded)
        {
            // Since we're grounded, we're no longer jumping
            m_IsJumpActive = false;
            m_PlayerJumped?.Invoke(false);
            
            // Stun-animation
            m_Animator.SetBool("isStunned", false);
            // Jump-animation
            m_Animator.SetBool("isJumping", !m_GroundCheck.m_IsGrounded);
            
            // Play the landing sound
            SoundManager.PlayRandomSound(SoundType.LAND, m_LandVolume);
            
            // If the jump-buffer was successfully used, execute a jump!
            if (m_CanBufferJump)
            {
                Debug.Log("<b><color=grey>[FORGIVENESS]</color> Jump-Buffer Used!</b>");
                ExecuteJump();
            }
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!m_IsJumpActive)
        {
            // If the Coyote-Timer is already in effect, stop it so it can be reset
            if (m_CCoyoteTime != null)
            {
                StopCoroutine(m_CCoyoteTime);
            }
            // Execute the Coyote-Timer
            m_CCoyoteTime = StartCoroutine(C_CoyoteTime());
        }
    } // For when the player isn't grounded but also isn't jumping

    private void FixedUpdate()
    {
        // Animation-checks
        m_Animator.SetFloat("xVelocity", Mathf.Abs(m_RB.linearVelocityX));
        m_Animator.SetFloat("yVelocity", m_RB.linearVelocityY);
        m_Animator.SetBool("isJumping", !m_GroundCheck.m_IsGrounded);
        if (m_IsStunned == false)
        {
            m_Animator.SetBool("isStunned", false);
        }
        HandleSpriteFlipping();
    } // Used for animation-parameters
    
    #endregion

    public Vector2 GetVelocity()
    {
        return m_RB.linearVelocity;
    }
}
