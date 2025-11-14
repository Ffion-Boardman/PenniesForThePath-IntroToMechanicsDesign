using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private int value;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure that the collider is the player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("<b><color=grey>[GAME]</color> <color=yellow>Coin collected!</color></b>");
            
            // Playing the right coin sound
            SoundManager.PlaySound(SoundType.COIN, 1, 1);
            
            Destroy(gameObject);
        }
        
        //if (collision.transform.parent.TryGetComponent<ScoreTracker>(out var scoreTracker))
        //{
        //    scoreTracker.ApplyScore(value, this);
        //    // Play sound effect here
        //    // Play VFX here
        //    Destroy(gameObject);
        //}
    }
}
