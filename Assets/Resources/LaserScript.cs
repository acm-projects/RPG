using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
    public float cooldownTime = 10f;
    private float cooldownTimer = 0f;
    public int damageAmount = 10;

    public void ShootLaser(Vector3 position, Vector3 direction) {
        if (cooldownTimer <= 0f) {
            GameObject laserPrefab = Resources.Load<GameObject>("Laser");
            if (laserPrefab != null) {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);

                GameObject laser = Instantiate(laserPrefab, position, rotation);

                laser.SetActive(true);

                Destroy(laser, 2.0f);

                cooldownTimer = cooldownTime;
            }
            else {
                Debug.LogError("Laser prefab not found");
            }
        }
    }

    private void Update() {
        if (cooldownTimer > 0f) {
            cooldownTimer -= Time.deltaTime;
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(damageAmount);
            }
            
        }
    }

}