using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public float attackDelay;
    public Transform weaponTransform;
    public float weaponRange;
    public float weaponDamage;
    public LayerMask enemyLayer;

    void Update()
    {
        // Change condition to check for Q key press
        if (Input.GetKeyDown(KeyCode.W))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {

        yield return new WaitForSeconds(attackDelay);
        Collider2D enemy = Physics2D.OverlapCircle(weaponTransform.position, weaponRange, enemyLayer);
        if (enemy != null && enemy.GetComponent<EnemyAbstract>() != null)
        {
            // Convert weaponDamage to int before passing
            int damageInt = (int)weaponDamage;
            enemy.GetComponent<EnemyAbstract>().TakeDamage(damageInt);
        }
    }
}
