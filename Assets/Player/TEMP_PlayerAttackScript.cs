using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TEMP_PlayerAttackScript : MonoBehaviour
{
    public GameObject player;
    public GameObject projectile;
    public Transform projectilePos;
    public int attackDamage;

    public Vector3 nearestEnemy;

    private List<GameObject> allEnemiesInRange = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && allEnemiesInRange.Count > 0) {
            float enemyDistance;
            nearestEnemy = allEnemiesInRange[0].transform.position;
            foreach (GameObject currEnemy in allEnemiesInRange) { // find nearest enemy
                enemyDistance = Vector3.Distance(currEnemy.transform.position, player.transform.position); //get distance from player to current enemy
                if (enemyDistance < Vector3.Distance(nearestEnemy, player.transform.position))
                    nearestEnemy = currEnemy.transform.position;
            }
            shootProjectile();
        }
    }


    void shootProjectile() {
        Instantiate(projectile, projectilePos.position, Quaternion.identity);
        //projectile.GetComponent<TEMP_playerprojectilescript>().AimAtEnemy();
        projectile.GetComponent<TEMP_playerprojectilescript>().projectileDamage = attackDamage;
        
    }


     public List<GameObject> GetColliders () { return allEnemiesInRange; }
 
     void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Tobor"))
            allEnemiesInRange.Add(other.gameObject); 
     }
 
     void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.CompareTag("Enemy") || other.gameObject.CompareTag("Tobor"))
            allEnemiesInRange.Remove(other.gameObject);
     }
}
