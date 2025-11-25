using UnityEngine;

public class PlayerParallaxMove : MonoBehaviour
{
    [SerializeField] private Rigidbody2D m_RB;
    [SerializeField] private float m_MoveSpeed;
    
    void FixedUpdate()
    {
        m_RB.linearVelocityX = m_MoveSpeed;
    }
}
