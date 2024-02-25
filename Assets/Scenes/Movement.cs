using System.Collections;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    float vertical;
    float horizontal;

    public float moveSpeed;

    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [SerializeField] private TrailRenderer tr;

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector2(horizontal * moveSpeed, vertical * moveSpeed);

        // Flipping the character based on movement direction
        if (horizontal < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // Flip character horizontally
        }
        else if (horizontal > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // Character faces right
        }

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
        {
            StartCoroutine(Dash());
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
        rb.velocity = new Vector2(GetFacingDirection() * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity; 
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }

    int GetFacingDirection()
    {
        if (transform.localScale.x > 0)
        {
            return 1; // Facing right
        }
        else
        {
            return -1; // Facing left
        }
    }
}
