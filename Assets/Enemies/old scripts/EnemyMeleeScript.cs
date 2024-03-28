using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeScript : MonoBehaviour
{
    public Animator animator;
    public Collider2D attackPoint;
    public Rigidbody2D rb;
    private GameObject player;

    public float attackInterval = 1;
    public float attackDelay = 2;
    public int attackDamage = 5;

    public float knockbackForce = 200f;
    public float knockbackTime = 0.75f;
    
    public int enemyPursuingRange = 7;
    public int enemyAttackRange = 2;
    public float chaseSpeed = 2;
    
    private float timerHit = 0;
    private float timerHitDelay = 0;
    private float distance = 5;

    private bool facingRight = false; 
    

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, player.transform.position);
        Vector2 direction = player.transform.position - transform.position; 

        //face correct direction
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

        if (timerHitDelay <= 0) //if not attacking, move
            transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        if (distance < enemyPursuingRange) //if in range of character
        {
            timerHit += Time.deltaTime;
            timerHitDelay -= Time.deltaTime;
            animator.SetBool("isPursuing", true);
            if (timerHit > attackInterval && distance < enemyAttackRange) { //if can attack
                timerHit = 0;
                timerHitDelay = attackDelay;
                meleeAttack(direction);
            }
        } else {
            timerHitDelay = 0;
            animator.SetBool("isPursuing", false);
        }
    }

    /* Enemy does melee attacks at the player
     */
    void meleeAttack(Vector2 direction) {
        animator.SetTrigger("isAttacking");

        if (attackPoint.IsTouching(player.GetComponent<Collider2D>())) { //if attack hits player
            StartCoroutine(player.GetComponent<playerTempScript>().takeDamage(attackDamage, direction.normalized * knockbackForce, knockbackTime));
        }
    }
}
