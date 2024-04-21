using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour
{
    public int currentEnemyCount;
    public GameObject gameStateCanvas;
    private GameStateManager gameStateManager;
    [SerializeField] private bool portalOverride = true;

    void Start(){
        gameStateManager = gameStateCanvas.GetComponent<GameStateManager>();
    }
    void Update() {
        currentEnemyCount = GameObject.FindGameObjectsWithTag ("Enemy").Length;
        if (canEnterNextLevel()) {
            gameObject.GetComponent<CircleCollider2D>().isTrigger = true;
        }
    }

    public int GetCurrentEnemyCount() { return currentEnemyCount; }

    //If all enemies are dead, can transition to next level
    public bool canEnterNextLevel() {
        if (portalOverride)
            return true;
        return (currentEnemyCount <= 0);
    }

    //when player enters portal, goes to next level
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        { //if player enters portal
            gameStateManager.nextLevelWithTransition();
        }
    }
}
