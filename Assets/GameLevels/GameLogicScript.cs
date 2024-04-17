using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLogicScript : MonoBehaviour
{
    private static float levelMultiplier = 0.5f;
    public static int gameLevel = 0;
    private static float enemyMultiplier = 1;
    private static float spawnMultiplier = 1.5f;

    public static void enterNextLevel() {
        if (gameLevel <= 3)
            gameLevel++;
    }

    public static void resetLevels() {
        gameLevel = 0;
    }

    public static void setGameLevel (int level) {
        gameLevel = level;
    }

    public static float getEnemyMultiplier() {
        return enemyMultiplier + levelMultiplier * gameLevel;
    }

    public static float getSpawnMultiplier() {
        return spawnMultiplier + levelMultiplier * gameLevel;
    }

    public static void setEnemyMultiplier(int newVal) {
        enemyMultiplier = newVal;
    }
    public static void setSpawnMultiplier(int newVal) {
        spawnMultiplier = newVal;
    }

    public static void setLevelMultiplier(int newVal) {
        levelMultiplier = newVal;
    }

    
}
