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
    public GameObject skillsUI;
    public GameObject gameOverUI;


    
    void Start () {
        Time.timeScale = 1;

        gameOverUI.SetActive(false);
        MenuItems(false);
        GameItems(true);
    }

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

        GameItems(false);
        Time.timeScale = 0f;
    }

    public void Resume() {
        MenuItems(false);
        GameItems(true);

        Time.timeScale = 1;
    }

    public void LoadMenu () {
        Debug.Log ("Switched to Main Menu...");
        SceneManager.LoadScene(0); //goes to main menu
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    // Open and Closes Settings Menu
    public void Settings (bool flag) {
        settingsMenuUI.SetActive(flag);
    }

    // Open and Closes Help Menu
    public void Help (bool flag) {
        helpMenuUI.SetActive(flag);
    }

    // Resets current level
    public void RestartScene() {
        gameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Activates Game Over screen
    public void GameOver() {
        gameIsPaused = true;
        gameOverUI.SetActive(true);
        GameItems(false);
        Time.timeScale = 0f;
    }

    private void MenuItems(bool flag) {
        pauseMenuUI.SetActive(flag);
        settingsMenuUI.SetActive(flag);
        helpMenuUI.SetActive(flag);
    }
    private void GameItems(bool flag) {
        playerHPBar.SetActive(flag);
        skillsUI.SetActive(flag);
    }
}
