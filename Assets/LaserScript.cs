using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour
{
   public LineRenderer lineRenderer;
   public Transform laserPosition;
   public float laserMaxLength = 10f;

   private Transform target;

   private void Start() {
    lineRenderer.enabled = false;
    target = GameObject.FindGameObjectWithTag("Player").transform;
   }

   private void Update() {
    Vector3 direction = (target.position - laserPosition.position).normalized;
    RaycastHit2D hit = Physics2D.Raycast(laserPosition.position, direction, laserMaxLength);
    lineRenderer.SetPosition(0, laserPosition.position);
    /*if (Input.GetKeyDown(KeyCode.Space)) {
        if (lineRenderer.isVisible == true) {
            lineRenderer.enabled = false;
        }
        else {
            lineRenderer.enabled = true;
        }
    }*/
    if (hit.collider != null) {
        if (hit.collider.CompareTag("Player")) {
            PlayerHealth playerHealth = hit.collider.GetComponent<PlayerHealth>();
            if (playerHealth != null) {
                playerHealth.TakeDamage(10);
            }
        }
        lineRenderer.SetPosition(1, hit.point);
    }
    else {
        lineRenderer.SetPosition(1, laserPosition.position + direction * laserMaxLength);
    }
   }

   public void SetLaserVisibility(bool isVisible) {
    lineRenderer.enabled = isVisible;
   }
}
