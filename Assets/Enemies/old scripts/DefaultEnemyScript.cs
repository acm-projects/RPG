using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyScript : MonoBehaviour
{
    public GameObject projectile;
    private GameObject player;
    public Transform projectilePos;
    public Animator animator;
    
    public float projectileInterval = 1;
    public int enemyPursuingRange = 12;
    public int enemyAttackRange = 8;
    public float chaseSpeed = 2;
    public int attackDamage = 5;
    
    private float timer = 0;
    private float distance = 5;
    private Vector2 direction;

    private bool facingRight = false; 
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        distance = Vector2.Distance(transform.position, player.transform.position);
        direction = player.transform.position - transform.position; 

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
        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

         if (distance < enemyPursuingRange)
        {
            animator.SetBool("isPursuing", true);
            if (timer > projectileInterval && distance < enemyAttackRange) {
                timer = 0;
                shoot();
            }
            
        } else {
            animator.SetBool("isPursuing", false);
        }
    }

    public void shoot() {
        animator.SetTrigger("isAttacking");
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
        projectile.GetComponent<EnemyProjectileMovementScript>().projectileDamage = attackDamage;
    }
}
