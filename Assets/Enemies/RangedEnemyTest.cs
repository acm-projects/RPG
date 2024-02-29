using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemyTest : EnemyAbstract
{  
    public GameObject projectile;
    public Transform projectilePos;

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    protected override void Attack() {
        animator.SetTrigger("isAttacking");
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
        projectile.GetComponent<EnemyProjectileMovementScript>().projectileDamage = attackDamage;  
    }

    protected override void PursuingAction(bool state) {
        animator.SetBool("isPursuing", state);
    }
}
