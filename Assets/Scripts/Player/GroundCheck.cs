using System;
using Unity.VisualScripting;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Transform m_RaycastPosition;
    [SerializeField] private LayerMask m_GroundLayer;
    public bool m_IsGrounded;

    private void FixedUpdate()
    {
        //m_IsGrounded = Physics2D.Raycast(m_RaycastPosition.position, Vector2.down, 0.1f, m_GroundLayer);
        m_IsGrounded = Physics2D.OverlapBox(m_RaycastPosition.position, new Vector2(.9f, .2f), 0.1f, m_GroundLayer);
    }

    private void OnDrawGizmos()
    {
        if (m_IsGrounded)
        {
            Gizmos.color = Color.yellow;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        Gizmos.DrawCube(m_RaycastPosition.position, new Vector2(.9f, .2f));
    }
}