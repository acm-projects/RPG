using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : MonoBehaviour, IDamageable
{
    public int maxHealth = 1000;
    public int currentHealth;
    public HealthBar healthBar;
    public Patrol bossPatrolScript;

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

        if (currentHealth <= 100) {
            bossPatrolScript.TransitionToPhase2();
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
    }

    void Die() {
        Destroy(gameObject, 1.3f);
    }

    public int Health { get { return currentHealth; } set {currentHealth = value; }}
}
