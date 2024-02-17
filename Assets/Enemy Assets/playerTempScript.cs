using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTempScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb; 
    private Vector2 movement; 
    public int health = 100;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Current HP: " + health);
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        rb.transform.Translate(Vector2.up *Time.deltaTime * movement.y * moveSpeed);
        rb.transform.Translate(Vector2.right *Time.deltaTime * movement.x * moveSpeed);
    }

    /*void FixedUpdate() {
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime); 
    }*/

    /* Reduces MC health by given amount
     *
     * hpLoss the damage taken
     */
    public IEnumerator takeDamage(int hpLoss, Vector2 knockback, float delay) {
        health -= hpLoss; 
        rb.AddForce(knockback, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;

        //Debug.Log("Knockback = " + knockback);
        Debug.Log("Current HP: " + health);
    }

    public void takeDamage(int hpLoss) {
        health -= hpLoss; 
        Debug.Log("Current HP: " + health);
    }
}
