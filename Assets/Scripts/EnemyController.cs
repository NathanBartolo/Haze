using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int maxHealth = 20;
    private int currentHealth;
    public float moveSpeed = 2f;  // Speed at which the enemy moves
    public GameObject deathEffect; // Optional: Effect to play on death

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float attackRange = 2f;  // The range at which the enemy can attack the player
    public int attackDamage = 5;
    public float attackCooldown = 1f;
    private bool isAttacking = false;
    private Transform player;

    public float followRange = 5f;  // The range at which the enemy starts following the player
    public AudioClip deathSFX; // Drag the enemy death sound effect here
    private AudioSource audioSource;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player").transform;  // Find the player by tag
    }

    void Update()
    {
        if (player != null && !isAttacking)
        {
            // Calculate the distance to the player
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            // If the player is within follow range, follow the player
            if (distanceToPlayer <= followRange)
            {
                FollowPlayer(distanceToPlayer);
            }

            // If the player is within attack range, attack and stop following
            if (distanceToPlayer <= attackRange)
            {
                if (!isAttacking)
                {
                    StartCoroutine(AttackPlayer());  // Start attacking the player
                }
            }
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        StartCoroutine(FlashOnHit());

        if (currentHealth <= 0)
        {
            Die();
      
        }
    }

    void Die()
    {
        PlaySFX(deathSFX);

        if (deathEffect != null)
        {
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        Destroy(gameObject);
        
    }

    void FollowPlayer(float distanceToPlayer)
    {
        // Only move towards the player if they are within follow range and not too close (avoid constant following if already in attack range)
        if (distanceToPlayer > attackRange)
        {
            Vector3 direction = (player.position - transform.position).normalized;  // Get direction to player
            transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);

            // Set the enemy's facing direction to the player
            if (direction.x > 0)
            {
                spriteRenderer.flipX = true;  // Facing right
            }
            else if (direction.x < 0)
            {
                spriteRenderer.flipX = false;   // Facing left
            }
        }
    }

    System.Collections.IEnumerator FlashOnHit()
    {
        spriteRenderer.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = Color.white;
    }

    System.Collections.IEnumerator AttackPlayer()
    {
        isAttacking = true;
        animator.SetTrigger("Attack");

        yield return new WaitForSeconds(attackCooldown);

        if (Vector2.Distance(transform.position, player.position) <= attackRange)
        {
            player.GetComponent<PlayerHealth>().TakeDamage(attackDamage);  // Call the player's health script to take damage
        }

        isAttacking = false;
    }

    void PlaySFX(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound effect once
        }
    }
}
