using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script is responsible for transitioning the player from the current level to the next one, after they reach the end. This was taken from this video: https://www.youtube.com/watch?v=-7I0slJyi8g

public class LevelTransitionScript : MonoBehaviour
{
    // The level transition relies on the player character stepping into a loading zone. After the player enters into the trigger area, the scene manager loads the next scene, according to the build index.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1, LoadSceneMode.Single);
        }
    }
}
