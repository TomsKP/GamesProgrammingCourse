using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This script is responsible for the main menu button functionality. The methods themselves are called by using the Unity editor OnClick() functionality. This was taken from: https://www.youtube.com/watch?v=-GWjA6dixV4
public class MainMenuScript : MonoBehaviour
{

    // This method will load the next scene, according to the build index. This will be the introduction screen. 
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1, LoadSceneMode.Single);
    }

    // This will close the application.
    public void QuitGame()
    {
        Application.Quit();
    }
}
