using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDelay = 0.5f; // Default value
    public Transform weaponTransform;
    public float weaponRange;
    public float weaponDamage;
    public LayerMask enemyLayer;
    public Animator playerAnim; // Fixed typo here

    private bool isAttacking = false; // Added a flag to prevent multiple attacks

    void Update()
    {
        // Change condition to check for Q key press
        if (Input.GetKeyDown(KeyCode.Q) && !isAttacking) // Fixed key and added check for isAttacking
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {

        isAttacking = true; // Set to true before the attack starts

        playerAnim.SetTrigger("isAttack");

        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(weaponTransform.position, weaponRange, enemyLayer); // Changed to OverlapCircleAll
        foreach (Collider2D enemy in hitEnemies)
        {
            if (enemy != null && enemy.GetComponent<EnemyAbstract>() != null)
            {
                int damageInt = Mathf.RoundToInt(weaponDamage); // Convert weaponDamage to int using Mathf.RoundToInt
                enemy.GetComponent<EnemyAbstract>().TakeDamage(damageInt);
            }
        }

        //playerAnim.Play("SLASHAttack"); // Play the animation
        
   

        yield return new WaitForSeconds(playerAnim.GetCurrentAnimatorStateInfo(0).length); // Wait for animation to finish


        isAttacking = false; // Set to false when the attack ends
    }
}