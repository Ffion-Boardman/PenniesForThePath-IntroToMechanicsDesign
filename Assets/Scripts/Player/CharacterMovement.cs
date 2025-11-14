using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region Values
    
    [Header("External Variables")]
    [SerializeField] GroundCheck m_GroundCheck;
    [SerializeField] private Animator m_Animator;
    
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
    
    /// <summary>
    /// This event is called for the sake of the Collider-Controller script
    /// </summary>
    public event Action<bool> m_PlayerJumped;
    
    [Header("Movement Sounds")]
    [SerializeField] [Range(0, 1)] public float m_JumpVolume;
    [SerializeField] [Range(0, 1)] public float m_FootstepVolume;

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
    
    #endregion
    
    private void Awake()
    {
		m_RB = GetComponent<Rigidbody2D>();
        m_OriginalScaleX = transform.localScale.x;
    }
    
    #region Movement
    public void SetInMove(float direction)
    {
        m_InMove = direction;
        
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
    
    // Flips the sprite for the character-animator
    private void HandleSpriteFlipping()
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

    private bool isStunned = false;
    IEnumerator C_MoveUpdate()
    {
        while (m_IsMoveActive)
        {
            yield return new WaitForFixedUpdate();
            
            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;  
            
            /*
            //if (!isStunned)
            //{
            //    m_RB.linearVelocityX = m_MoveSpeed * m_InMove;   
            //}
            // If we're actively jumping then we'll want to do an add-force so the jump doesn't feel weird
            if (m_IsJumpActive)
            {
                if (Mathf.Abs(m_RB.linearVelocityX) < m_MoveSpeed)
                {
                    // F = m*a, we have the player's velocity, and a mass of 1
                    m_RB.AddForceX(m_MoveSpeed * m_InMove * m_MoveForceMultiplier, ForceMode2D.Force); 
                }
            }
            else if (Mathf.Abs(m_RB.linearVelocityX) < m_MoveSpeed)
            {
                m_RB.AddForceX(m_MoveSpeed * m_InMove * m_MoveForceMultiplier, ForceMode2D.Force);   
            }
            else
            {
                m_RB.linearVelocityX = m_MoveSpeed * m_InMove;  
            }
            */
            
        }
    }

    public void MoveCancelled()
    {
        m_RB.linearVelocityX = 0;
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
        // PUT SYSTEM-CALLER HERE
        
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

    public void JumpCancelled()
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
    }

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
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (m_GroundCheck.m_IsGrounded)
        {
            // Since we're grounded, we're no longer jumping
            m_IsJumpActive = false;
            m_PlayerJumped?.Invoke(false);
            
            // Jump-animation
            m_Animator.SetBool("isJumping", !m_GroundCheck.m_IsGrounded);
            
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
    }

    private void FixedUpdate()
    {
        // Movement animations
        m_Animator.SetFloat("xVelocity", Mathf.Abs(m_RB.linearVelocityX));
        m_Animator.SetFloat("yVelocity", m_RB.linearVelocityY);
        HandleSpriteFlipping();
    }
    
    #endregion
}
