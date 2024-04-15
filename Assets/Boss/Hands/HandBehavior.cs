using UnityEngine;
using System.Collections;

public class HandBehavior : MonoBehaviour
{
    public enum HandState
    {
        Idle,
        Entering,
        Active,
        Charging
    }

    public HandState currentState = HandState.Idle;

    public float entrySpeed = 5f;
    public float chargeSpeed = 20f;
    public Vector3 targetPosition; // The target position in the scene where the hand stops
    public Vector3 hiddenPosition; // The position where the hand is hidden off-screen
    public int peekCount = 3;
    private GameObject player;
    private int currentPeek = 0;
    public int damage = 10;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        switch (currentState)
        {
            case HandState.Entering:
                MoveToPosition(targetPosition, entrySpeed);
                break;
            case HandState.Active:
                // No active moving in this case, just idle waiting
                break;
            case HandState.Charging:
                // Charging logic might be handled elsewhere
                break;
            case HandState.Idle:
                // Nothing necessary here, just idle
                break;
        }
    }

    public void EnterScene(Vector3 startPosition, Vector3 endPosition)
    {
        transform.position = startPosition; // Set the starting position
        targetPosition = endPosition; // Set the target position
        hiddenPosition = startPosition;
        currentState = HandState.Entering; // Change state to Entering
    }

    IEnumerator PeekBehavior()
    {
        yield return new WaitForSeconds(1); // Wait a bit before starting peek-a-boo

        while (currentPeek < peekCount)
        {
            // Move to hidden position
            MoveToPosition(hiddenPosition, entrySpeed);
            yield return new WaitForSeconds(1); // Wait for 1 second

            // Move back to target position
            MoveToPosition(targetPosition, entrySpeed);
            yield return new WaitForSeconds(1); // Wait for 1 second

            currentPeek++;
        }

        currentState = HandState.Charging; // Change state to Charging
        ChargeAtPlayer();
    }

    void ChargeAtPlayer()
    {
        Vector3 chargeDirection = (player.transform.position - transform.position).normalized;
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = chargeDirection * chargeSpeed;
            StartCoroutine(ResetCharge());
        }
    }

    IEnumerator ResetCharge()
    {
        yield return new WaitForSeconds(3); // Give some time after charging

        // Stop the hand's movement by setting the velocity to zero
        Rigidbody2D rb2d = GetComponent<Rigidbody2D>();
        if (rb2d != null)
        {
            rb2d.velocity = Vector2.zero;  // Reset velocity to stop moving
        }

        currentPeek = 0; // Reset the peek count
        currentState = HandState.Idle; // Change state to Idle

        // Optionally add a delay before becoming active again
        yield return new WaitForSeconds(5); // Idle for 5 seconds before becoming active again
        currentState = HandState.Entering; // Make the hand enter the scene again or become active
    }

    void MoveToPosition(Vector3 position, float speed)
    {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, position, step);

        // Check if the destination is reached
        if (Vector3.Distance(transform.position, position) <= 0.001f)
        {
            if (currentState == HandState.Entering)
            {
                currentState = HandState.Active; // Transition to Active state where it waits
                StartCoroutine(PeekBehavior());
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth playerHealth = other.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
            }
        }
    }

}
