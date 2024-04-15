using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 5f; 
    [SerializeField] private Animator animator;

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

    //Returns true if player is shielded
    public bool IsShielded()
    {
        return shielded;
    }

    //Triggered when object enters player's collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if player collides with HealingPotion
        if (other.CompareTag("HealingPotion"))
        {
            // call Heal method from PlayerHealth script
            playerHealth.Heal(20);
            // Destroy HealingPotion
            Destroy(other.gameObject);
        }
        // Check if player enters transition  tile
        if (other.CompareTag("EndTile"))
        {
            Debug.Log("EndTile");
            Destroy(other.gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
    }

    //Player dash
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

    //Activates shield if player isn't shielded and presses E
    void CheckShield()
    {
        if (Input.GetKey(KeyCode.E) && !shielded)
        {
            shield.SetActive(true);
            shielded = true;
            Invoke("NoShield", 5f);
        }
    }

    //Deactivates shield
    void NoShield()
    {
        shield.SetActive(false);
        shielded = false;
    }

    /* Reduces MC health by given amount WITH knockback
     *
     * hpLoss the damage taken
     * knockback the strength of the knockback
     * delay the time the player is knockedback for before stopping
     */
    public IEnumerator KnockbackDamage(int hpLoss, Vector2 knockback, float delay)
    {
        playerHealth.TakeDamage(hpLoss); //reduces HP

        //knockback animation
        rb.AddForce(knockback, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
    }
}