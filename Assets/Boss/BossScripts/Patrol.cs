using System.Collections;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    public enum CharacterState
    {
        Intro,
        Idle,
        Patrolling,
        Attacking,
        Charging,
        Staggering
    }

    public float speed = 2.0f;
    public Vector2 patrolBoundsMin;
    public Vector2 patrolBoundsMax;
    private Vector2 currentDirection = Vector2.right;
    private CharacterState currentState = CharacterState.Intro;
    private float originalY;
    private float patrolTime = 0f;
    private float timeBetweenAttacks = 10f;
    public GameObject player;
    private float chargeAttackTimer = 0f;
    private float timeBetweenChargeAttacks = 15f;
    public GameObject laserPrefab;
    private float laserAttackTimer = 0f;
    private float timeBetweenLaserAttacks = 5f;
    private bool isInPhase2 = false;
    public BossHealth bossHealth;
    public GameObject firstHand;
    public GameObject secondHand;

    void Start()
    {
        originalY = transform.position.y;
        StartCoroutine(IntroBehavior());
    }

    void Update()
    {
        switch (currentState)
        {
            case CharacterState.Intro:
                // Intro is handled by the Coroutine
                break;
            case CharacterState.Idle:
                FloatEffect();
                break;
            case CharacterState.Patrolling:
                PatrolArea();
                patrolTime += Time.deltaTime;
                chargeAttackTimer += Time.deltaTime;
                laserAttackTimer += Time.deltaTime;
                if (patrolTime >= timeBetweenAttacks)
                {
                    patrolTime = 0;
                    StartCoroutine(AttackPatternX());
                }
                else if (chargeAttackTimer >= timeBetweenChargeAttacks)
                {
                    chargeAttackTimer = 0;
                    StartCoroutine(ChargeAttack());
                }
                if (laserAttackTimer >= timeBetweenLaserAttacks)
                {
                    laserAttackTimer = 0;
                    SpawnMinion();
                }
                break;
            case CharacterState.Staggering:
                break;
            case CharacterState.Attacking:
                break;
            case CharacterState.Charging:
                break;
        }

    }

    void FloatEffect()
    {
        // Sinusoidal floating effect
        float floatHeight = Mathf.Sin(Time.time) * 0.5f; // Adjust amplitude for effect strength
        transform.position = new Vector3(transform.position.x, originalY + floatHeight, transform.position.z);
    }

    IEnumerator IntroBehavior()
    {
        float introTime = 3.0f;  // Extended time for a longer intro phase
        float amplitude = 30.0f;  // Increased amplitude for a wider range
        float frequency = 3.0f;  // Adjusted frequency for appropriate speed

        float startTime = Time.time;
        while (Time.time - startTime < introTime) {
            // Use a sine function to create a smooth oscillation over a longer range
            float x = Mathf.Sin((Time.time - startTime) * frequency) * amplitude;
            // Ensure the movement is centered around the original horizontal position
            transform.position = new Vector3(originalY + x, transform.position.y, transform.position.z);
            yield return null;
        }
        currentState = CharacterState.Idle;
        StartCoroutine(TransitionToPatrol());
    }

    IEnumerator TransitionToPatrol()
    {
        yield return new WaitForSeconds(2); // Idle for 2 seconds before patrolling
        currentState = CharacterState.Patrolling;
    }

    void PatrolArea()
    {
        transform.Translate(currentDirection * speed * Time.deltaTime);
        if ((currentDirection.x > 0 && transform.position.x >= patrolBoundsMax.x) || 
            (currentDirection.x < 0 && transform.position.x <= patrolBoundsMin.x))
        {
            FlipDirection();
        }
    }

    void FlipDirection()
    {
        currentDirection.x = -currentDirection.x;
    }

    IEnumerator AttackPatternX()
    {
        currentState = CharacterState.Attacking;

        // Define start and end points for the first line of the "X"
        Vector3 startPoint1 = new Vector3(Screen.width, Screen.height, 0); // Top right
        Vector3 endPoint1 = new Vector3(0, 0, 0); // Bottom left
        // Convert screen points to world points
        startPoint1 = Camera.main.ScreenToWorldPoint(startPoint1);
        endPoint1 = Camera.main.ScreenToWorldPoint(endPoint1);
        startPoint1.z = endPoint1.z = 0; // Ensure z-coordinate is suitable for your game

        // Define start and end points for the second line of the "X"
        Vector3 startPoint2 = new Vector3(0, Screen.height, 0); // Top left
        Vector3 endPoint2 = new Vector3(Screen.width, 0, 0); // Bottom right
        // Convert screen points to world points
        startPoint2 = Camera.main.ScreenToWorldPoint(startPoint2);
        endPoint2 = Camera.main.ScreenToWorldPoint(endPoint2);
        startPoint2.z = endPoint2.z = 0; // Ensure z-coordinate is suitable for your game

        float attackDuration = 2.0f; // Duration of the attack
        float timer = 0;
        float rotationSpeed = 360f; // Degrees per second

        // Move from top right to bottom left
        while (timer <= attackDuration)
        {
            float progress = timer / attackDuration;
            transform.position = Vector3.Lerp(startPoint1, endPoint1, progress);
            // Rotate crazily
            transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        timer = 0; // Reset timer for the next movement

        // Reset rotation before next movement for visual consistency
        transform.rotation = Quaternion.identity;

        // Move from top left to bottom right
        while (timer <= attackDuration)
        {
            float progress = timer / attackDuration;
            transform.position = Vector3.Lerp(startPoint2, endPoint2, progress);
            // Rotate crazily
            transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;
        }

        // After attack, return to another state, e.g., Patrolling
        transform.rotation = Quaternion.identity; // Reset rotation to original after attack
        
        // Calculate a return position at the top center of the screen
        // Adjust the y value as needed to position the boss inside the screen bounds
        Vector3 screenTopCenter = new Vector3(Screen.width / 2, Screen.height, 0);
        Vector3 returnPosition = Camera.main.ScreenToWorldPoint(screenTopCenter);
        returnPosition.z = 0; // Make sure the boss is placed on the correct z-axis
        returnPosition.y -= 1; // Adjust this value to ensure the boss is not off-screen

        // Optionally, add a slight delay before moving back to the top
        yield return new WaitForSeconds(1);

        // Smoothly move the boss back to the top center position
        float returnDuration = 2.0f; // Duration to move back to the top
        timer = 0; // Reset timer for the return movement

        Vector3 startPosition = transform.position; // Current position after attack

        while (timer <= returnDuration)
        {
            float progress = timer / returnDuration;
            transform.position = Vector3.Lerp(startPosition, returnPosition, progress);
            timer += Time.deltaTime;
            yield return null;
        }

        currentState = CharacterState.Patrolling;
    }

    IEnumerator ChargeAttack()
    {
        currentState = CharacterState.Charging; // Use Charging state to differentiate
        
        // Signal charge phase (could visually indicate this with a simple effect)
        yield return new WaitForSeconds(2); // Simulate charge up time
        
        Vector3 startPosition = transform.position;
        Vector3 endPosition = player.transform.position; // Target the player
        
        float chargeDuration = 1.0f; // Adjust for desired speed
        float timer = 0;
        
        while (timer <= chargeDuration)
        {
            float progress = timer / chargeDuration;
            transform.position = Vector3.Lerp(startPosition, endPosition, progress);
            timer += Time.deltaTime;
            yield return null;
        }
        
        currentState = CharacterState.Patrolling; // Return to patrolling after the charge
    }

    void SpawnMinion()
    {
        GameObject minion = Instantiate(laserPrefab, transform.position, Quaternion.identity);
        minion.transform.up = Vector3.up;

    }

    public void TransitionToPhase2()
    {
        if (!isInPhase2)
        {
            isInPhase2 = true;
            // Example visual change: Change color to red. Adjust as needed.
            GetComponent<SpriteRenderer>().color = Color.red;
            
            if (bossHealth != null)
            {
                int healthIncrease = 600;
                bossHealth.currentHealth += healthIncrease;
                bossHealth.healthBar.SetMaxHealth(bossHealth.maxHealth);
                bossHealth.healthBar.SetHealth(bossHealth.currentHealth);
            }
            // Adjust attack patterns or behavior for phase 2
            // For example, increase attack speed or change the attack sequence
            speed *= 2; // Example: Double the speed
            timeBetweenAttacks /= 2; // Example: More frequent attacks
            timeBetweenChargeAttacks /= 2;
            timeBetweenLaserAttacks /= 2;

            // Additional logic for phase 2 transition, such as new attacks
            Vector3 leftStartPos = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 2, 0));
            Vector3 rightStartPos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height / 2, 0));
            leftStartPos.z = rightStartPos.z = 0; // Ensure correct Z position

            // Target positions for the hands to stop
            Vector3 leftTargetPos = new Vector3(-7, 3, 0); // Adjust as needed
            Vector3 rightTargetPos = new Vector3(7, 3, 0); // Adjust as needed

            // Activate and move hands into the scene
            HandBehavior leftHandBehavior = firstHand.GetComponent<HandBehavior>();
            HandBehavior rightHandBehavior = secondHand.GetComponent<HandBehavior>();

            if (leftHandBehavior != null && rightHandBehavior != null)
            {
                leftHandBehavior.EnterScene(leftStartPos, leftTargetPos);
                rightHandBehavior.EnterScene(rightStartPos, rightTargetPos);
            }

            firstHand.SetActive(true);
            secondHand.SetActive(true);
        }
    }

    IEnumerator StaggeredMovement() {
        int numberOfMovements = 5;
        float moveDuration = 0.5f;
        float waitTime = 1.05f;

        for (int i = 0; 0 < numberOfMovements; i++) {
            Vector3 randomDirection = new Vector3(Random.Range(-1, 2), Random.Range(-1, 2), 0).normalized;
            Vector3 movePosition = transform.position + randomDirection * 3; // Move 3 units in the random direction

            float timer = 0;
            while (timer < moveDuration) {
                transform.position = Vector3.MoveTowards(transform.position, movePosition, speed * Time.deltaTime);
                timer += Time.deltaTime;
                yield return null;
            }

            // Wait after moving before the next move
            yield return new WaitForSeconds(waitTime);
        }

        currentState = CharacterState.Patrolling;
    }
}