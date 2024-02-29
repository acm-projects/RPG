using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    //Checks if player presses ESCAPE to pause/unpause game
    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            gameIsPaused = !gameIsPaused;
            if (gameIsPaused)
                Pause();
            else 
                Resume();
        }
    }

    public void Pause() {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1;
    }

    public void LoadMenu () {
        Debug.Log ("Switched to Main Menu...");
    }

    public void Settings () {
        Debug.Log ("Opened Settings...");
    }

    public void Help () {
        Debug.Log ("Opened Help...");
    }
}
