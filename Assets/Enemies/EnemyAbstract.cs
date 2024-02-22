using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyAbstract : MonoBehaviour, IDamageable
{
    public int maxHealth;
    public int currentHealth;

    public Animator animator;
    public Rigidbody2D rb;
    GameObject player;

    public float attackInterval;
    public int attackDamage;
    
    public int enemyPursuingRange;
    public int enemyAttackRange;
    public float chaseSpeed;
    
    float attackTimer = 0;
    float direction;

    bool facingRight = false; 

    public void initializeEnemy () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    
    // Start is called before the first frame update
    void Start()
    {
        initializeEnemy();
    }


    /* Reduces enemy health by given amount
     *
     * damage the damage taken
     */
    public void TakeDamage (int damage) {
        currentHealth -= damage; 
    }

    public int Health { get { return currentHealth; } set {currentHealth = value; }}

    public abstract void Attack();
}
