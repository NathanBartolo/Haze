using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public int maxJumps = 2; // Maximum double jumps
    private int jumpCount = 0; // Tracks how many jumps the player has made

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool isCrouching;
    private bool isJumping = false;
    private bool isFalling = false;

    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Animator animator; // Reference to the Animator component

    private Vector3 respawnPosition; // To store the checkpoint position
    public GameObject hitbox; // Reference to the hitbox GameObject

    // Audio clips for different SFX
    public AudioClip jumpSFX;
    public AudioClip attackSFX;
    public AudioClip hitSFX;
    public AudioClip PlayCheckpointSFX;

    private AudioSource audioSource; // AudioSource component to play SFX

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>(); // Get the Animator component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component

        respawnPosition = transform.position; // Save the initial position

        if (hitbox != null)
        {
            hitbox.SetActive(false); // Ensure the hitbox is disabled initially
        }

        // Initialize the AudioSource
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        Move();
        Jump();
        HandleAttack();
        UpdateAnimations(); // Update animations based on player state
    }

    void Move()
    {
        if (isCrouching) return;
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 move = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
        rb.velocity = move;

        // Flip sprite based on movement direction
        if (horizontalInput > 0)
        {
            spriteRenderer.flipX = false; // Face right
        }
        else if (horizontalInput < 0)
        {
            spriteRenderer.flipX = true; // Face left
        }

        // Update the "Speed" parameter in the Animator based on horizontal input
        animator.SetFloat("Speed", Mathf.Abs(horizontalInput)); // Speed is set to the absolute value of horizontal input
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && (isGrounded || jumpCount < maxJumps))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            jumpCount++;

            // Play Jump SFX
            PlaySFX(jumpSFX);
        }
    }

    // Handle attacking input
    void HandleAttack()
    {
        if (Input.GetKeyDown(KeyCode.K)) // Trigger attack on key press
        {
            animator.SetTrigger("AttackTrigger"); // Play attack animation

            if (hitbox != null)
            {
                StartCoroutine(EnableHitboxDuringAttack());
            }

            // Play Attack SFX
            PlaySFX(attackSFX);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("LightPlatform"))
        {
            jumpCount = 0;
            isGrounded = true;

            
        }

        // Handle Shadow Traps (always deadly)
        if (collision.gameObject.CompareTag("ShadowTrap"))
        {
            Respawn(); // Player dies when touching a ShadowTrap
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("LightPlatform"))
        {
            isGrounded = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Checkpoint logic
        if (collision.CompareTag("Checkpoint"))
        {
            respawnPosition = collision.transform.position; // Update the respawn position
            Debug.Log("Checkpoint updated: " + respawnPosition);

            // Trigger checkpoint animation
            PlaySFX(PlayCheckpointSFX);
        }

        // Handle traps
        if (collision.CompareTag("ShadowTrap") || collision.CompareTag("LightTrap"))
        {
            Respawn(); // Player dies when hitting a trap
        }
    }

    // Respawn the player at the last checkpoint
    public void Respawn()
    {
        transform.position = respawnPosition;
        rb.velocity = Vector2.zero; // Reset velocity

        // Play Hit SFX
        PlaySFX(hitSFX);

        Debug.Log("Player Respawned at Checkpoint!");
    }

    

    IEnumerator EnableHitboxDuringAttack()
    {
        // Activate the hitbox at the start of the animation
        hitbox.SetActive(true);

        // Wait for the length of the attack animation
        yield return new WaitForSeconds(0.5f); // Adjust duration to match your animation

        // Disable the hitbox after the animation ends
        hitbox.SetActive(false);
    }

    // Update jumping and falling animations based on vertical velocity
    void UpdateAnimations()
    {
        if (isGrounded)
        {
            // Player is on the ground
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }
        else
        {
            // Player is in the air
            if (rb.velocity.y > 0)
            {
                // Rising up (jumping)
                animator.SetBool("isJumping", true);
                animator.SetBool("isFalling", false);
            }
            else if (rb.velocity.y < 0)
            {
                // Falling down
                animator.SetBool("isJumping", false);
                animator.SetBool("isFalling", true);
            }
        }
    }

    // Play SFX with the given AudioClip
    void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }
}
