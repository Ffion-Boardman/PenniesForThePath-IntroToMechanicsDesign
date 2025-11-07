using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterMovement : MonoBehaviour
{
    #region Values
    
    [Header("Basic Movement")]
    // Variables
    private Rigidbody2D m_RB;
    [SerializeField] private float m_MoveSpeed;
	[SerializeField] private float m_JumpStrength;
    [SerializeField] GroundCheck m_GroundCheck;
    // Movement
    private float m_InMove;
    private PlayerControls m_ActionMap;
    // Movement Update
    private Coroutine m_CMove;
    private bool m_IsMoveActive;
    // Jump Update
    private Coroutine m_CJump;
    private bool m_IsJumpActive = false;
    /// <summary>
    /// This event is called for the sake of the Collider-Controller script
    /// </summary>
    public event Action<bool> m_PlayerJumped;

    [Header("Forgiveness Mechanics")]
    // Coyote Time
    [SerializeField] [Range(0, 0.5f)] private float m_CoyoteTimeThreshold = 0.4f;
    private Coroutine m_CCoyoteTime;
    private bool m_CanCoyoteJump = false;
    // Jump Buffer
    [SerializeField] [Range(0, 0.5f)] private float m_JumpBufferThreshold = 0.4f;
    private Coroutine m_CJumpBuffer;
    private bool m_CanBufferJump = false;
    // Variable Jump Height
    private float m_VariableJumpHeightDamper = 2.0f;
    
    //[Header("Particles")]
    //[SerializeField] private ParticleSystem m_JumpParticles;
    
    //[Header("Sound")]
    
    #endregion
    
    private void Awake()
    {
		m_RB = GetComponent<Rigidbody2D>();
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

    IEnumerator C_MoveUpdate()
    {
        while (m_IsMoveActive)
        {
            yield return new WaitForFixedUpdate();
            m_RB.linearVelocityX = m_MoveSpeed * m_InMove;
        }
    }

    public void MoveCancelled()
    {
        if (m_RB.linearVelocity.x > 0)
        {
            
        }
    }

    #endregion

    #region Jump
    
    private void ExecuteJump() // When the player ACTUALLY jumps
    {
        // Jump-force
        m_RB.AddForce(Vector2.up * m_JumpStrength, ForceMode2D.Impulse);
        // Jump particles
        //m_JumpParticles.Play();
        // Jump sound
        
        // Jump-bool
        m_IsJumpActive = true;
        // Jump-event
        m_PlayerJumped?.Invoke(true);
        // Jump-update
        //StartCoroutine(C_JumpUpdate());
    }
    
    public void JumpPerformed() // When the player ATTEMPTS to jump (presses space)
    {
        // If jump isn't already active, AND if we're grounded, OR we're within coyote-limits: jump immediately
        if ( (m_GroundCheck.m_IsGrounded || m_CanCoyoteJump))
        {
            if (m_CanCoyoteJump) { Debug.Log("Coyote-Time Used!"); }
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
            
            // If the jump-buffer was successfully used, execute a jump!
            if (m_CanBufferJump)
            {
                Debug.Log("Jump Buffer Used!");
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
    
    #endregion
}
