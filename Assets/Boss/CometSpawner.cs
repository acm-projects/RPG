using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CometSpawner : MonoBehaviour
{
    public GameObject cometPrefab; // Assign this in the inspector with your comet GameObject
    public float minSpawnDelay = 1.0f; // Minimum delay between spawns
    public float maxSpawnDelay = 5.0f; // Maximum delay between spawns

    void Start()
    {
        StartCoroutine(SpawnComets());
    }

    private IEnumerator SpawnComets()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minSpawnDelay, maxSpawnDelay));
            GameObject cometInstance = Instantiate(cometPrefab, GenerateSpawnPosition(), Quaternion.identity);
            CometBehavior cometBehavior = cometInstance.GetComponent<CometBehavior>();
            if (cometBehavior != null)
            {
                float angle = Random.Range(-45f, -45); // Randomize angles between -45 and 45 degrees
                cometBehavior.SetAngle(angle);
                cometBehavior.speed = Random.Range(5f, 15f); // Randomize speed between 5 and 15
            }
        }
    }

    private Vector3 GenerateSpawnPosition()
    {
        // Example: Randomize position. Adjust according to your game's logic.
        float x = Random.Range(-10f, 10f); // Assuming you want it to appear within these bounds
        float y = 5f; // Assuming you want it to spawn off the top of the screen
        return new Vector3(x, y, 0); // Adjust z based on your game's needs
    }
}
