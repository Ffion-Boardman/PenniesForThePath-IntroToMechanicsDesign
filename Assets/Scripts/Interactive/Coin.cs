using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float value;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Make sure that the collider is the player
        if (collision.CompareTag("Player"))
        {
            Debug.Log("Coin collected!");
            SoundManager.PlaySound(SoundType.COIN);
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
