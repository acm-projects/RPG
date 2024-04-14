using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerTempScript : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Rigidbody2D rb;
    public Animator animator;
    private Vector2 movement;
    //public int health = 100;
    //bool facingRight = true;

    public PlayerHealth playerHealth;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //get user input
        //movement.x = Input.GetAxisRaw("Horizontal");
        //movement.y = Input.GetAxisRaw("Vertical");

        //move character
        //rb.transform.Translate(Vector2.up *Time.deltaTime * movement.y * moveSpeed);
        //rb.transform.Translate(Vector2.right *Time.deltaTime * movement.x * moveSpeed);

        
    }

    /* Reduces MC health by given amount WITH knockback
     *
     * hpLoss the damage taken
     * 
     */
    public IEnumerator takeDamage(int hpLoss, Vector2 knockback, float delay)
    {
        playerHealth.TakeDamage(hpLoss); //reduces HP

        //knockback animation
        rb.AddForce(knockback, ForceMode2D.Impulse);
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
    }

    /* Reduces MC health by given amount
     *
     * hpLoss the damage taken
     */
    public void takeDamage(int hpLoss)
    {
        playerHealth.TakeDamage(hpLoss); //reduces HP
    }


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
        if (other.CompareTag("EndTile"))
        {
            Debug.Log("EndTile");
            Destroy(other.gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex);
        }
    }
}
