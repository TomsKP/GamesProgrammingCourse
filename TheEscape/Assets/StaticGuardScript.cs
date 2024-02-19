using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;

// This script is responsible for the static guards.
// The calculations for the angles were combined from several videos discussing field of view implementation: 
// https://www.youtube.com/watch?v=j1-OyLo77ss
// https://www.youtube.com/watch?v=CSeUMTaNFYk

public class Guard : MonoBehaviour
{
    public PlayerControl player; // Stores the player character script. Used to access the spotted variable.
    public GameObject playerCharacter; // Stores the player character game object
    public Transform playerTransform; // Stores player character Transform component
    public float detectionRadius = 5f; // Radius for detecting the player character
    public float detectionAngle = 90f; // The angle for detecting the player character
    public float changeInterval = 10f; // The interval used to make the guard change the direction they are facing

    public Vector2[] directions; // Stores the directions the guard will cycle through
    public Sprite[] sprites; // Stores the sprites for the 4 directions the guard can face
    public SpriteRenderer spriteRenderer; // Stores the spriteRenderer component for the guard
    public GameObject spottedNotice; // Stores the pop-up object, used to indicate that the guard spotted the player character

    private int index = 0; // The index for cycling through the directions array
    private float timeSinceChange = 0f; // Used for timing the direction changes

    private void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player"); // Stores the player character data
        player = playerCharacter.GetComponent<PlayerControl>(); // Stores the player control script
        playerTransform = playerCharacter.transform; // Stores the player character transfrom component
        spriteRenderer = GetComponent<SpriteRenderer>(); // Stores the SpriteRenderer component
    }

    private void Update()
    {
        timeSinceChange += Time.deltaTime; // Used as a timer. More can be read about this here: https://docs.unity3d.com/ScriptReference/Time-deltaTime.html

        Vector2 directionToPlayer = playerTransform.position - transform.position; // Finds the direction to the player.
        float distanceToPlayer = directionToPlayer.magnitude; // Finds the distance to the player.
        float angleToPlayer = Vector2.Angle(directions[index], directionToPlayer); //Finds the angle to the player, based on which direction the guard is facing, and the direction the player.

        // If the distance to the player is less than or equals to the detection radius of the guard, and the angle is within the detection angle / 2, checks if the player is visible.
        if (distanceToPlayer <= detectionRadius && angleToPlayer <= detectionAngle / 2f)
        {
            // If the player is standing in light, the game is over, and the level restarts.
            if (player.visible)
            {
                player.spotted = true; // Spotted is set to true, to stop the player character from moving.
                spottedNotice.SetActive(true); // The pop-up above the guard becomes visible.
                Invoke("RestartLevel", 1); // Invokes the RestartLevel method after one second to give the player time to see where they messed up.
            }
        }
        
        // This section sets the guard sprite to represent which way they are facing.
        if (directions[index] == Vector2.up)
        {
            spriteRenderer.sprite = sprites[0];
        }
        else if (directions[index] == Vector2.right) 
        {
            // Makes sure the sprite is not flipped when the guard is facing right.
            if (spriteRenderer.flipX == true)
            {
                spriteRenderer.flipX = false;
            }
            spriteRenderer.sprite = sprites[1];
            
        }
        else if (directions[index] == Vector2.down)
        {
            spriteRenderer.sprite = sprites[2];
        }
        else if (directions[index] == Vector2.left)
        {
            // Flips the sprite when the guard is facing left.
            if(spriteRenderer.flipX == false)
            {
                spriteRenderer.flipX = true;
            }
            spriteRenderer.sprite = sprites[3];
        }

        // When the timer reaches the interval set for the guard, the index increases, which makes the guard change direction. 
        if (timeSinceChange >= changeInterval)
        {
            timeSinceChange = 0;

            index++;

            // Sets the index back to 0, if the end of the array is reached, making it loop infinitely.
            if(index >= directions.Length)
            {
                index = 0;
            }
        }



    }

    // This makes it so the player loses if they collide with the guard.
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            player.spotted = true; // Sets spotted to true, to stop the player character from moving.
            spottedNotice.SetActive(true); // Makes the pop-up above the guard visible.
            Invoke("RestartLevel", 1); // Inovkes the RestartLevel method after 1 second.
        }
    }

    // Loads the current level. 
    public void RestartLevel()
    {
        player.spotted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }

    // Draws a sphere for the detection radius in the Unity editor. I wanted to draw the detection angle as well, but I couldn't figure out how, and it wasn't important enough to spend any more time on. Angles for the guards are eyeballed.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}

