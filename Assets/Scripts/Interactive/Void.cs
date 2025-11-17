using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class Void : MonoBehaviour
{
    [SerializeField] private int m_damage;
    [SerializeField] private CinemachineCamera  m_camera;
    private Coroutine m_VoidCoroutine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("<b><color=grey>[GAME]</color> <color=red>Spike hit!</color></b>");

        
        
        // Kill the player
        if (collision.transform.parent.TryGetComponent<HealthComponent>(out var healthComponent))
        {
            healthComponent.ApplyDamage(m_damage, this);
        }
    }
    
    IEnumerator C_VoidCoroutine()
    {
        // Wait so the player can see the sprite-change
        yield return new WaitForSeconds(2);
        
        // Stop tracking the player and just let them fall
        m_camera.Target.TrackingTarget = null;
    }
}