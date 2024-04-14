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
    public GameObject pauseButton;

    public GameObject[] skillInfoMenus;

    public GameObject levelTransition;


    
    void Start () {
        Time.timeScale = 1;

        gameOverUI.SetActive(false);
        MenuItems(false);
        GameItems(true);
        levelTransition.SetActive(false);
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

    //Pauses game
    public void Pause() {
        pauseMenuUI.SetActive(true);

        GameItems(false);
        Time.timeScale = 0f;
    }

    //Resumes game
    public void Resume() {
        MenuItems(false);
        GameItems(true);

        Time.timeScale = 1;
    }

    //Sets scene to main menu
    public void LoadMenu () {
        Debug.Log ("Switched to Main Menu...");
        SceneManager.LoadScene(0); //goes to main menu
        gameIsPaused = false;
        Time.timeScale = 1;
    }

    public void changeLevel (int level) {
        
        SceneManager.LoadScene(level);
    }

    public void nextLevel() {
        levelTransition.SetActive(true);
        levelTransition.GetComponent<Animator>().SetTrigger("LevelTransition");
    }

    public void goToNextLevel () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1); //gets next active scene
    }

    // Open and Closes Settings Menu
    private void Settings (bool flag) {
        settingsMenuUI.SetActive(flag);
    }

    // Open and Closes Help Menu
    private void Help (bool flag) {
        helpMenuUI.SetActive(flag);
    }

    // Resets current level
    public void RestartScene() {
        gameIsPaused = false;
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Opens skill information popup
    public void OpenSkillInfo (int skill) {
        skillInfoMenus[skill].SetActive(true);
    }

    // Activates Game Over screen
    public void GameOver() {
        gameIsPaused = true;
        gameOverUI.SetActive(true);
        GameItems(false);
        Time.timeScale = 0f;
    }

    // Sets all menu screens to active or unactive (based on flag)
    private void MenuItems(bool flag) {
        pauseMenuUI.SetActive(flag);
        settingsMenuUI.SetActive(flag);
        helpMenuUI.SetActive(flag);
        for (int i = 0; i < skillInfoMenus.Length; i++) {
            skillInfoMenus[i].SetActive(flag);
        }
    }

    //Sets all on screen game items (Canvas type) to active or unactive (based on flag)
    private void GameItems(bool flag) {
        playerHPBar.SetActive(flag);
        skillsUI.SetActive(flag);
        pauseButton.SetActive(flag);
    }


}
