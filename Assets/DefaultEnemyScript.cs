using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefaultEnemyScript : MonoBehaviour
{
    public GameObject projectile;
    public GameObject player;
    public Transform projectilePos;
    public Animator animator;
    
    public float projectileInterval = 1;
    public int enemyRange = 7;
    public int chaseSpeed = 2;
    
    private float timer = 0;
    private float distance = 5;
    private Vector2 direction;
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

        transform.position = Vector2.MoveTowards(this.transform.position, player.transform.position, chaseSpeed * Time.deltaTime);

        if (distance < enemyRange && timer > projectileInterval)
        {
            animator.SetBool("isAttacking", true);
            timer = 0;
            shoot();
        } else if (distance > enemyRange) {
            animator.SetBool("isAttacking", false);
        }
    }

    public void shoot() {
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
    }
}
