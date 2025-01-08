using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Hitbox : MonoBehaviour
{
    public int damage = 10; // Damage dealt by the hitbox

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the hitbox collides with an enemy
        if (collision.CompareTag("Enemy"))
        {
            // Example: Apply damage to the enemy
            EnemyController enemy = collision.GetComponent<EnemyController>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Debug.Log("Hit " + collision.name);
        }
    }


}