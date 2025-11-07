using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float value;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Coin collected!");
        Destroy(gameObject);
        
        //if (collision.transform.parent.TryGetComponent<ScoreTracker>(out var scoreTracker))
        //{
        //    scoreTracker.ApplyScore(value, this);
        //    // Play sound effect here
        //    // Play VFX here
        //    Destroy(gameObject);
        //}
    }
}
