using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This script is responsible for the epilogue screen functionality.
public class OutroScript : MonoBehaviour
{
    private float timePassed = 0f; // This variable is used for the timer. It stores how much time has passed.
    public GameObject prompt; // This stores the game object for the "Press Any Button To Continue" prompt

    // Update is called once per frame
    void Update()
    {
        timePassed += Time.deltaTime; // This line acts as a timer. More can be read about this here: https://docs.unity3d.com/ScriptReference/Time-deltaTime.html

        // This sets the prompt to active after 3 seconds have passed, giving the user instruction to continue.
        if (timePassed > 3f && prompt.activeSelf == false)
        {
            prompt.SetActive(true);
        }

        // This checks for any key input after 3 seconds have passed. If the user presses any button, they will be sent back to the main menu.
        if (Input.anyKeyDown && timePassed > 3f)
        {
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }
}
