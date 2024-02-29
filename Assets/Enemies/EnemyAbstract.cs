using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* To use, animator must have an "isPursuing" boolean variable
 */

public abstract class EnemyAbstract : MonoBehaviour, IDamageable
{
    public int maxHealth;
    public int currentHealth;

    public Animator animator;
    public Rigidbody2D rb;
    GameObject player;

    public float attackInterval;
    public float attackMovementDelay;
    public int attackDamage;

    //public float knockbackForce;
    //public float knockbackTime;
    
    public int enemyPursuingRange;
    public int enemyAttackRange;
    public float chaseSpeed;
    
    float timerAttackInterval = 0;
    float timerAttackDelay = 0;
    Vector2 direction;

    bool facingRight = false; 

    public void InitializeEnemy () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Movement() {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position; 

        //face correct direction (LEFT and RIGHT)
        if (direction.x > 0 && !facingRight) {
            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1;
            transform.localScale = tempScale;
            facingRight = true;
        } else if (direction.x < 0 && facingRight) {
            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1;
            transform.localScale = tempScale;
            facingRight = false;
        }

        if (timerAttackDelay <= 0) //if not attacking, move
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        if (distance < enemyPursuingRange) //if in range of character
        {
            timerAttackInterval += Time.deltaTime;
            timerAttackDelay -= Time.deltaTime;
            PursuingAction(true);
            if (timerAttackInterval > attackInterval && distance < enemyAttackRange) { //if can attack + in range
                timerAttackInterval = 0;
                timerAttackDelay = attackMovementDelay;
                Attack();
            }
        } else {
            timerAttackDelay = 0;
            PursuingAction(false);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeEnemy();
    }


    /* Reduces enemy health by given amount
     *
     * damage the damage taken
     */
    public void TakeDamage (int damage) {
        currentHealth -= damage; 
    }

    public int Health { get { return currentHealth; } set {currentHealth = value; }}

    protected abstract void Attack();
    protected abstract void PursuingAction(bool state);
}
