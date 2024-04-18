using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRangeEnemy : EnemyAbstract
{   
    //melee attacks
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform projectilePos;


    // Start is called before the first frame update
    void Start()
    {
        DefaultRandomRangeEnemy();
        InitializeEnemy();
    }

    // Update is called once per frame
    void Update()
    {
        DefaultMovement();
    }

    protected override void AttackingAction() {
        animator.SetTrigger("isAttacking");
        DefaultRangedAttack(projectile, projectilePos);
    }

    protected override void PursuingAction() {
        animator.SetBool("isPursuing", true);
        DefaultPursuingAction();
    }

    protected override void IdleAction () {
        animator.SetBool("isPursuing", false);
    }
}
