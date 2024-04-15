using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionScript : MonoBehaviour
{
    public void goToNextLevel () {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1); //gets next active scene
    }
}