using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeEnemy : EnemyAbstract
{
    public Collider2D attackPoint; 

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();
    }

    protected override void AttackingAction() {
        animator.SetTrigger("isAttacking");
        DefaultMeleeAttack(attackPoint);
    }

    protected override void PursuingAction() {
        animator.SetBool("isPursuing", true);
        DefaultPursuingAction();
    }

    protected override void IdleAction () {
        animator.SetBool("isPursuing", false);
    }
}
