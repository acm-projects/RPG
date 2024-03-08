using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;
    public GameObject settingsMenuUI;
    public GameObject helpMenuUI;
    public GameObject playerHPBar;

    //private GameObject helpMenuUI;
    public GameObject gameOverUI;

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
        playerHPBar.SetActive(false);
        Time.timeScale = 0f;
    }

    public void Resume() {
        pauseMenuUI.SetActive(false);
        settingsMenuUI.SetActive(false);
        playerHPBar.SetActive(true);
        Time.timeScale = 1;
    }

    public void LoadMenu () {
        Debug.Log ("Switched to Main Menu...");
    }

    public void Settings () {
        Debug.Log ("Opened Settings...");
        settingsMenuUI.SetActive(true);
    }

    public void SettingsExit () {
        settingsMenuUI.SetActive(false);
    }
    public void Help () {
        Debug.Log ("Opened Help...");
        helpMenuUI.SetActive(true);
    }
    public void HelpExit () {
        helpMenuUI.SetActive(false);
    }

    public void RestartScene() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void GameOver() {
        gameOverUI.SetActive(true);
        //Time.timeScale = 0f;
    }
}
