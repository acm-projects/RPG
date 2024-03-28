using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f; // Adjust this to control player speed
    private PlayerHealth playerHealth; // Reference PlayerHealth script

    void Start() {
        playerHealth = GetComponent<PlayerHealth>(); // Get PlayerHealth script
    }
    void Update()
    {
        // Input
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Movement
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f) * speed * Time.deltaTime;
        transform.position += movement;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // Check if player collides with HealingPotion
        if (other.CompareTag("HealingPotion")) {
            // call Heal method from PlayerHealth script
            playerHealth.Heal(10);
            // Destroy HealingPotion
            Destroy(other.gameObject);
        }
    }
}
