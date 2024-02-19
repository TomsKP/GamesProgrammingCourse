using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This script is responsible for the pause screen functionaliy. This was taken from this video: https://www.youtube.com/watch?v=9dYDBomQpBQ

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu; // This stores the game object of the pause menu itself. 
    public bool isPaused = false; // This stores whether or not the game is currently paused.

    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false); // The pause menu is set to be off, when the game starts.
    }

    // Update is called once per frame
    void Update()
    {
        // When the user presses the "Esc" button and the game is unpaused, the game will pause. If the game is paused, it will unpause.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    // This method pauses the game by setting the pause screen to be active, and setting the time scale to 0. This makes all internal timers freeze. More can be read about this here: https://docs.unity3d.com/ScriptReference/Time-timeScale.html
    public void PauseGame()
    {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }


    // This method resumes the game by deactivating the pause screen and resetting the time scale to 1. More can be read about this here: https://docs.unity3d.com/ScriptReference/Time-timeScale.html
    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }


    // This method shuts the application down.
    public void QuitGame()
    {
        Application.Quit();
    }


    // This method returns the player to the main menu. The time scale is also set back to 1, as it will not be automatically reset when starting a new game from the main menu.
    public void returnToMainMenu()
    {
        isPaused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene(0, LoadSceneMode.Single); 
    }
}
