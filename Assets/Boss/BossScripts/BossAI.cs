using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [Range(1, 15)]
    [SerializeField]
    private float viewRadius = 11;
    [SerializeField]
    private float detectionCheckDelay = 0.1f;
    [SerializeField]
    private Transform target = null;
    [SerializeField]
    private LayerMask PlayerLayerMask;
    [SerializeField]
    private LayerMask visibilityLayer;

    public LaserScript laserScript;
    public bool IsPlayerDetected { get; private set; }

    private void Start() {
        StartCoroutine(DetectionCoroutine());
    }

    private void Update() {
        if (target != null) {
            IsPlayerDetected = CheckIfTargetIsVisible();
        }
        else {
            IsPlayerDetected = false;
        }

        if (IsPlayerDetected && laserScript != null && target != null) {
            Vector3 direction = (target.position - transform.position).normalized;
            laserScript.ShootLaser(transform.position, direction);
        }
        
    }

    private bool CheckIfTargetIsVisible() {
        var result = Physics2D.Raycast(transform.position, target.position - transform.position, viewRadius, visibilityLayer);
        if (result.collider != null) {
            return (PlayerLayerMask & (1 << result.collider.gameObject.layer)) != 0;
        }
        return false;
    }

    private void DetectIfOutOfRange() {
        if (target == null || target.gameObject.activeSelf == false || Vector2.Distance(transform.position, target.position) > viewRadius) {
            target = null;
        }
    }

    private void CheckIfPlayerIsInRange() {
        Collider2D collision = Physics2D.OverlapCircle(transform.position, viewRadius, PlayerLayerMask);
        if (collision != null) {
            target = collision.transform;
        }
    }

    private IEnumerator DetectionCoroutine() {
        /*yield return new WaitForSeconds(detectionCheckDelay);
        DetectTarget();
        StartCoroutine(DetectionCoroutine());*/
        while (true) {
            CheckIfPlayerIsInRange();
            DetectIfOutOfRange();
            yield return new WaitForSeconds(detectionCheckDelay);
        }
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, viewRadius);
    }
}
