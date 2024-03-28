using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : EnemyAbstract
{  
    public GameObject projectile;
    public Transform projectilePos;

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();
    }
    

    protected override void AttackingAction() {
        animator.SetTrigger("isAttacking");
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
        projectile.GetComponent<EnemyProjectileMovementScript>().projectileDamage = attackDamage;  
    }

    protected override void PursuingAction() {
        animator.SetBool("isPursuing", true);
        DefaultPursuingAction();
    }

    protected override void IdleAction () {
        animator.SetBool("isPursuing", false);
    }
}
