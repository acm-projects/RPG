using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStateManager : MonoBehaviour
{
    [SerializeField] private int gameLevel;
    [SerializeField] private static bool gameIsPaused = false;
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject settingsMenuUI;
    [SerializeField] private GameObject helpMenuUI;
    [SerializeField] private GameObject playerHPBar;
    [SerializeField] private GameObject skillsUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject pauseButton;

    [SerializeField] private GameObject[] skillInfoMenus;

    [SerializeField] private GameObject levelTransition;
    
    void Start () {
        Time.timeScale = 1; //starts time
        GameLogicScript.setGameLevel(gameLevel);

        //Activates and deactivates screen items
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
        Debug.Log("RESUME GAME");
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

    //Changes level (scene) to given level
    public void changeLevel (int level) {
        SceneManager.LoadScene(level);
    }

    //Changes level (scene) to next scene in build settings, triggers transition animation
    public void nextLevelWithTransition() {
        levelTransition.SetActive(true);
        levelTransition.GetComponent<Animator>().SetTrigger("LevelTransition");
    }

    //Changes level (scene) to next scene in build settings, NO transition animation
    public void nextLevel () {
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
        GameLogicScript.resetLevels(); 
        gameIsPaused = false;
        Time.timeScale = 1;
        PlayerHealthManager.currentPlayerHealth = PlayerHealthManager.currentResetHealth; //resets hp to value at start of level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Opens skill information popup based on given skill index
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
