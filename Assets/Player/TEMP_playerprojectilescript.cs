using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_playerprojectilescript : MonoBehaviour
{
    public float projectileSpeed = 15;
    public int renderRotation = 0;
    public int timeProjectileFlies = 10;
    public int projectileDamage;
    private float timer;

    private GameObject player;

    private Rigidbody2D rb;

    Vector3 target;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody2D>();
        gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
        target = player.GetComponentInChildren<TEMP_PlayerAttackScript>().nearestEnemy;

        AimAtEnemy();

        Vector3 direction = player.transform.position - transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeProjectileFlies)
        {
            Destroy(gameObject);
        }
    }

    public void AimAtEnemy () {
        Vector3 direction = target - transform.position;

        rb.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + renderRotation);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Enemy"))
        { //if hit enemy
            other.GetComponent<IDamageable>().TakeDamage(projectileDamage);
            Destroy(gameObject);
        }
        else if (other.gameObject.CompareTag("Wall"))
        { //if hit wall
            Destroy(gameObject);
        }
    }
}
