using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomMeleeEnemy : EnemyAbstract
{   
    //melee attacks
    public Collider2D attackPoint; 

    // Start is called before the first frame update
    void Start()
    {
        DefaultRandomMeleeEnemy();
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();
    }

    protected override void AttackingAction() {
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
