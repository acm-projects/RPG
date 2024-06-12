using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuScript : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject settingsMenuUI;
    public GameObject helpMenuUI;
    public GameObject levelTransition;

    void Start()
    {
        Time.timeScale = 1; //starts time 
        settingsMenuUI.SetActive(false);
        helpMenuUI.SetActive(false);
        levelTransition.SetActive(false);
        PlayerHealthManager.currentPlayerHealth = 100; //sets player HP to max
        PlayerHealthManager.currentResetHealth = 100; //sets reset hp to max as well
        GameLogicScript.resetLevels();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1); //gets next active scene
    }

    public void Settings()
    {
        Debug.Log("Opened Settings...");
        settingsMenuUI.SetActive(true);
    }

    public void SettingsExit()
    {
        settingsMenuUI.SetActive(false);
    }

    public void Help()
    {
        Debug.Log("Opened Help...");
        helpMenuUI.SetActive(true);
    }

    public void HelpExit()
    {
        helpMenuUI.SetActive(false);
    }

    public void QuitGame()
    {
        Debug.Log("QUIT!");
        Application.Quit();
    }

     public void nextLevel() {
        levelTransition.SetActive(true);
        levelTransition.GetComponent<Animator>().SetTrigger("LevelTransition");
    }
}
