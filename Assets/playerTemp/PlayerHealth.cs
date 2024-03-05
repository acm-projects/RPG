using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    public HealthBar healthBar;
    public Text healthText;
    public GameStateManager gameStateManager;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
        UpdateHealthText();
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(20);
        }*/

        if (currentHealth <= 0)
        {
            Die();
        }

    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
    }

    public void Heal(int amount) {
        // Increase currentHealth by amount
        currentHealth += amount;
        // Clamp current health to ensure it doesn't exceed max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Update health bar UI
        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
    }

    void UpdateHealthText() {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    void Die()
    {
        gameStateManager.GameOver();
    }
}
