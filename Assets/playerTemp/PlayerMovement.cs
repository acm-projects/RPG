using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 5f;

    public Animator animator;
    private PlayerHealth playerHealth;
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 10f;
    private float dashingTime = 0.1f;
    private float dashingCooldown = 0.2f;
    private Rigidbody2D rb;
    private bool shielded;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private GameObject shield;

    private Vector2 movement; 
    //public int health = 100;
    bool facingRight = true; 

    void Start()
    {
        playerHealth = GetComponent<PlayerHealth>();
        rb = GetComponent<Rigidbody2D>();
        shielded = false;
    }

    void Update()
    {
        CheckShield();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f) * speed * Time.deltaTime;
        transform.position += movement;

        animator.SetFloat("Horizontal", moveHorizontal);
        animator.SetFloat("Vertical", moveVertical);
        if (movement.x == 0 && movement.y == 0)
            animator.SetFloat("Speed", 0);
        else 
            animator.SetFloat("Speed", 1);

        //face correct direction (LEFT and RIGHT)
        if (moveHorizontal > 0 && !facingRight) {
            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1;
            transform.localScale = tempScale;
            facingRight = true;
        } else if (moveHorizontal < 0 && facingRight) {
            Vector3 tempScale = transform.localScale;
            tempScale.x *= -1;
            transform.localScale = tempScale;
            facingRight = false;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }
    }

    public bool IsShielded()
    {
        return shielded;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("HealingPotion"))
        {
            playerHealth.Heal(10);
            Destroy(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    void CheckShield()
    {
        if (Input.GetKey(KeyCode.E) && !shielded)
        {
            shield.SetActive(true);
            shielded = true;
            Invoke("NoShield", 5f);
        }
    }

    void NoShield()
    {
        shield.SetActive(false);
        shielded = false;
    }
}