using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    public HealthBar healthBar;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    public void Heal(int amount) {
        // Increase currentHealth by amount
        currentHealth += amount;
        // Clamp current health to ensure it doesn't exceed max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Update health bar UI
        healthBar.SetHealth(currentHealth);
    }
}
