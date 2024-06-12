using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] 
    private GameObject toborPrefab;

    [SerializeField]
    private float toborInterval = 5f;
    void Start()
    {
        StartCoroutine(SpawnEnemy(toborInterval, toborPrefab));
    }

    private IEnumerator SpawnEnemy(float interval, GameObject enemy) {
        yield return new WaitForSeconds(interval);
        GameObject newEnemy = Instantiate(enemy, new Vector3(Random.Range(-5f, 5f), Random.Range(-6f, 6f), 0), Quaternion.identity);
        StartCoroutine(SpawnEnemy(interval, enemy));
    }
}
