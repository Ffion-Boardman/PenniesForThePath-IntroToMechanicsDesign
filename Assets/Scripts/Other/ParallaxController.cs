using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float m_StartPosition, m_ImageLength;
    [SerializeField] public GameObject m_Camera;
    public float m_ParallaxEffect; // The speed that the background will move in relation to the camera
    
    void Start()
    {
        m_StartPosition = transform.position.x;
        m_ImageLength = 21.6f;
    }

    void FixedUpdate()
    {
        // Calculate the distance background will move based on camera movement
        // E.g., 0 = move with camera, 1 = won't move at all, 0.5 = half-move
        float distance = m_Camera.transform.position.x * m_ParallaxEffect;
        float movement = m_Camera.transform.position.x * (1 - m_ParallaxEffect);
        
        transform.position = new Vector3(m_StartPosition + distance, transform.position.y, transform.position.z);

        if (movement > m_StartPosition + m_ImageLength)
        {
            m_StartPosition += m_ImageLength;
        }
        else if (movement < m_StartPosition - m_ImageLength)
        {
            m_StartPosition -= m_ImageLength;
        }
    }
}
