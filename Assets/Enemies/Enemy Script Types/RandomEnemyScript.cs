using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyScript : EnemyAbstract
{   
    //melee attacks
    public Collider2D attackPoint; 
    //ranged attacks
    public GameObject projectile;
    public Transform projectilePos;

    public int enemyType; 


    // Start is called before the first frame update
    void Start()
    {
        enemyType = Random.Range(0, 2);
        //Attack stats
        attackInterval = Random.Range(0.5f, 1.5f);
        attackMovementDelay = Random.Range(0.1f, 0.8f);
        attackDamage = Random.Range(1, 5);

        //Player knockback stats
        knockbackForce = Random.Range(10, 20);
        knockbackTime = Random.Range(0.1f, 0.3f);
        
        //Movement behavior
        enemyPursuingRange = Random.Range(10, 12);
        switch (enemyType) {
            case 0: //MELEE
                enemyAttackRange = Random.Range(1,2);
                break;
            case 1: //RANGED
                enemyAttackRange = Random.Range(7, 10);;
                break;
        }
        
        chaseSpeed = Random.Range(1, 8);

        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();
    }

    protected override void AttackingAction() {
        animator.SetTrigger("isAttacking");
        switch (enemyType) {
            case 0: //MELEE
                DefaultMeleeAttack(attackPoint);
                break;
            case 1: //RANGED
                DefaultRangedAttack(projectile, projectilePos);
                break;
        }
    }

    protected override void PursuingAction() {
        animator.SetBool("isPursuing", true);
        DefaultPursuingAction();
    }

    protected override void IdleAction () {
        animator.SetBool("isPursuing", false);
    }
}
