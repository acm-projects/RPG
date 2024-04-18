using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    private int maxHealth = 100;
    [SerializeField] private int currentHealth;
    
    [SerializeField] private HealthBar healthBar;
    [SerializeField] private Text healthText;
    [SerializeField] private GameStateManager gameStateManager;
    private PlayerMovement playerMovement;

    void Start()
    {
        currentHealth = PlayerHealthManager.currentPlayerHealth; //saves value across scenes
        PlayerHealthManager.currentResetHealth = currentHealth; //value hp reset to on restart
        healthBar.SetMaxHealth(maxHealth);
        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
        playerMovement = GetComponent<PlayerMovement>(); 
    }

    void Update()
    {
        if (currentHealth <= 0)
        {
            Die();
        }

    }

    //Reduces currentHealth by given amount
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        currentHealth = Mathf.Max(currentHealth, 0);
        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
        PlayerHealthManager.currentPlayerHealth = currentHealth;
    }

    //Increases currentHealth by given amount, update health bar
    public void Heal(int amount) {
        // Increase currentHealth by amount
        currentHealth += amount;
        // Clamp current health to ensure it doesn't exceed max health
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        // Update health bar UI
        healthBar.SetHealth(currentHealth);
        UpdateHealthText();
        PlayerHealthManager.currentPlayerHealth = currentHealth;
    }

    //Updates health bar text to show health amount
    void UpdateHealthText() {
        healthText.text = currentHealth + " / " + maxHealth;
    }

    //When player dies, activate game over menu
    void Die()
    {
        gameStateManager.GameOver();
    }
}