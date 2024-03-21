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

    //Attack stats
    public float attackInterval;
    public float attackMovementDelay;
    public int attackDamage;

    //Player knockback stats
    public float knockbackForce;
    public float knockbackTime;
    
    //Movement behavior
    public int enemyPursuingRange;
    public int enemyAttackRange;
    public float chaseSpeed;
    
    //Timers
    float timerAttackInterval = 0;
    float timerAttackDelay = 0;
    Vector2 direction;

    bool facingRight = true; 

    public void InitializeEnemy () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
    }
    public void DefaultMovement() {
        float distance = Vector2.Distance(transform.position, player.transform.position); //distance from player
        direction = player.transform.position - transform.position; //direction 

        if (distance < enemyPursuingRange) { //if in PURSING range of character
            PursuingAction();

            timerAttackInterval += Time.deltaTime;
            timerAttackDelay -= Time.deltaTime;
            if (timerAttackInterval > attackInterval && distance < enemyAttackRange) { //if can attack + in range
                timerAttackInterval = 0;
                timerAttackDelay = attackMovementDelay;
                AttackingAction();
            }
        } else {
            IdleAction();
        }

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

    // Health setter and getter
    public int Health { get { return currentHealth; } set {currentHealth = value; }}

    // default moving after player pursue
    public void DefaultPursuingAction() {
        if (timerAttackDelay <= 0) //if not attacking, move
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);
    }

    // default ranged attack behavior
    public void DefaultRangedAttack(GameObject projectile, Transform projectilePos) {
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
        projectile.GetComponent<EnemyProjectileMovementScript>().projectileDamage = attackDamage;
    }

    // default melee attack behavior
    public void DefaultMeleeAttack(Collider2D attackPoint) {
        if (attackPoint.IsTouching(player.GetComponent<Collider2D>())) { //if attack hits player
            StartCoroutine(player.GetComponent<playerTempScript>().takeDamage(attackDamage, direction.normalized * knockbackForce, knockbackTime));
        }
    }

    protected abstract void IdleAction();
    protected abstract void AttackingAction();
    protected abstract void PursuingAction();
}
