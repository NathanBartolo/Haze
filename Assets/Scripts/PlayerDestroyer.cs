using UnityEngine;

public class PlayerDestroyer : MonoBehaviour
{
    public AudioClip deathSound; // Optional: Sound to play when player dies
    public GameObject deathEffect; // Optional: Particle effect or death animation

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the object colliding is tagged as "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();
            if (player != null)
            {
                // Trigger the respawn logic
                player.Respawn();

                // Optional: Play death sound
                if (deathSound != null)
                {
                    AudioSource.PlayClipAtPoint(deathSound, player.transform.position);
                }

                // Optional: Create a death effect (e.g., particles or animation)
                if (deathEffect != null)
                {
                    Instantiate(deathEffect, player.transform.position, Quaternion.identity);
                }
            }
        }
    }
}
