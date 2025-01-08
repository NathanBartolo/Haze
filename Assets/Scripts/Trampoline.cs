using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounceForce = 10f; // Force applied to the player
    private Animator animator; // Animator for the trampoline animations
    public AudioClip trampolineSFX; // Drag your trampoline sound effect here
    private AudioSource audioSource;

    void Start()
    {
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>(); // Get the AudioSource component

        if (audioSource == null)
        {
            Debug.LogError("No AudioSource found on the trampoline GameObject!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) // Check if the player lands on the trampoline
        {
            Rigidbody2D playerRb = collision.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Apply bounce force to the player
                playerRb.velocity = new Vector2(playerRb.velocity.x, bounceForce);

                // Play the trampoline sound effect
                PlaySFX(trampolineSFX);

                // Trigger trampoline bounce animation
                if (animator != null)
                {
                    animator.SetTrigger("Bounce");
                }
            }
        }
    }

    // Play sound effect
    void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }
}
