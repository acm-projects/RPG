using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborHealth : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    
    public HealthBar healthBar;

    private bool isDead = false;
    void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    void Update()
    {
        if (currentHealth <= 0) {
            Die();
        }
    }

    public void TakeDamage(int damage)
    {
        if (!isDead) {
            currentHealth -= damage;
            healthBar.SetHealth(currentHealth);
        }
    }

    void Die() {
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Die");
        isDead = true;
        Destroy(gameObject, 1.3f);
    }

}
