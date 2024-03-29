using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectileMovementScript : MonoBehaviour
{
    private GameObject player;
    private Rigidbody2D rb;
    public float projectileSpeed = 20;
    public int renderRotation = -45;
    public int timeProjectileFlies = 10;
    public int projectileDamage;
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.FindGameObjectWithTag("Player");

        Vector3 direction = player.transform.position - transform.position;
        rb.velocity = new Vector2(direction.x, direction.y).normalized * projectileSpeed;

        float rotation = Mathf.Atan2(-direction.y, -direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rotation + renderRotation);
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (timer > timeProjectileFlies) {
            Destroy(gameObject);
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) { //if hit player
            Destroy(gameObject);
            other.GetComponent<playerTempScript>().takeDamage(projectileDamage);
        } 

    }
}
