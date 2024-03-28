using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float life  = 3;

    void Awake() {
        Destroy(gameObject, life);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        ToborHealth toborHealth = collision.gameObject.GetComponent<ToborHealth>();
        BossHealth bossHealth = collision.gameObject.GetComponent<BossHealth>();
        if (toborHealth != null) {
            toborHealth.TakeDamage(10);
        }
        else if (bossHealth != null) {
            bossHealth.TakeDamage(10);
        }
        Destroy(gameObject);
    }
}
