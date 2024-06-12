using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;

public class CometBehavior : MonoBehaviour
{
    public float speed = 5f; // Speed of the comet
    public float lifetime = 10f; // How long before the comet disappears
    public float angle = 0f; // Angle of the comet's trajectory
    public int damageAmount = 10; // Damage dealt to the player

    void Start()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {

            Vector2 direction = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad));
            rb.velocity = direction * speed;
        }
        Destroy(gameObject, lifetime); // Destroy the comet after 'lifetime' seconds
    }

    public void SetAngle(float newAngle)
    {
        angle = newAngle;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
