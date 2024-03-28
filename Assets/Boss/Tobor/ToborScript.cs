using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToborScript : MonoBehaviour
{
    public string playerTag = "Player";
    public float speed;
    private Animator animator;
    private float distance;
    private GameObject playerObject;
    public int damageAmount = 10;

    void Start()
    {
        animator = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag(playerTag);
    }

    void Update()
    {
        MoveTowardsPlayer();
        UpdateAnimation();
    }

    void MoveTowardsPlayer()
    {
        if (playerObject != null)
        {
            distance = Vector2.Distance(playerObject.transform.position, transform.position);
            Vector2 direction = playerObject.transform.position - transform.position;
            transform.position = Vector2.MoveTowards(transform.position, playerObject.transform.position, speed * Time.deltaTime);
        }
    }

    void UpdateAnimation()
    {
        if (playerObject != null)
        {
            Vector2 direction = (playerObject.transform.position - transform.position).normalized;
            float horizontalInput = direction.x;
            float verticalInput = direction.y;

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                animator.SetFloat("Horizontal", horizontalInput);
                animator.SetFloat("Vertical", 0);
            }
            else
            {
                animator.SetFloat("Horizontal", 0);
                animator.SetFloat("Vertical", verticalInput);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageAmount);
            }
        }
    }
}
