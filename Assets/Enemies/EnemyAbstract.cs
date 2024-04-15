using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/* To use, animator must have an "isPursuing" boolean variable
 */

public abstract class EnemyAbstract : MonoBehaviour, IDamageable
{
    [SerializeField] protected int maxHealth;
    [SerializeField] protected int currentHealth;

    protected Animator animator;
    protected Rigidbody2D rb;
    protected GameObject player;

    //Attack stats
    [SerializeField] protected float attackInterval;
    [SerializeField] protected float attackMovementDelay;
    [SerializeField] protected int attackDamage;

    //Player knockback stats
    [SerializeField] protected float knockbackForce;
    [SerializeField] protected float knockbackTime;
    
    //Movement behavior
    [SerializeField] protected int enemyPursuingRange;
    [SerializeField] protected int enemyAttackRange;
    [SerializeField] protected float chaseSpeed;
    
    //Timers
    protected float timerAttackInterval = 0;
    protected float timerAttackDelay = 0;
    protected Vector2 direction;

    protected bool facingRight = true; //sprite facing right or not
    protected bool isDead = false;

    public void InitializeEnemy () {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
    }
    public void DefaultMovement() {
        if (!isDead) {
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

            animator.SetFloat("Vertical", direction.y);
            animator.SetFloat("Horizontal", direction.x);
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
        animator.SetTrigger("isHit");
        currentHealth -= damage; 
        
        if (currentHealth <= 0) {
            isDead = true;
            animator.SetTrigger("isDead");
            Destroy(gameObject); // **make this run after a death animation!!
        }
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
          if (!player.GetComponent<PlayerMovement>().IsShielded() && attackPoint.IsTouching(player.GetComponent<Collider2D>())) //hits player if not shielded
        {

            StartCoroutine(player.GetComponent<PlayerMovement>().KnockbackDamage(attackDamage, direction.normalized * knockbackForce, knockbackTime));
        }
    }

    public void DefaultRandomMeleeEnemy () {
        //Attack stats
        attackInterval = Random.Range(0.5f, 1.5f);
        attackMovementDelay = Random.Range(0.1f, 0.8f);
        attackDamage = Random.Range(1, 5);

        //Player knockback stats
        knockbackForce = Random.Range(10, 20);
        knockbackTime = Random.Range(0.1f, 0.3f);
        
        //Movement behavior
        enemyPursuingRange = Random.Range(10, 12);
        enemyAttackRange = Random.Range(1,2);
        
        chaseSpeed = Random.Range(1, 8);
    }

    public void DefaultRandomRangeEnemy () {
        //Attack stats
        attackInterval = Random.Range(0.5f, 1.5f);
        attackMovementDelay = Random.Range(0.1f, 0.8f);
        attackDamage = Random.Range(1, 5);

        //Player knockback stats
        knockbackForce = Random.Range(10, 20);
        knockbackTime = Random.Range(0.1f, 0.3f);
        
        //Movement behavior
        enemyPursuingRange = Random.Range(10, 12);
        
        enemyAttackRange = Random.Range(7, 10);;
        
        chaseSpeed = Random.Range(1, 8);
    }
    protected abstract void IdleAction(); // Called while enemy is idling
    protected abstract void AttackingAction(); // Called while enemy is attacking
    protected abstract void PursuingAction(); // Called while enemy is pursuing player
}
