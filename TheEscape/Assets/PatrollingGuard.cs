using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// This script is responsible for the patrolling guard functionality.
// The calculations for the angles were combined from several videos discussing field of view implementation: 
// https://www.youtube.com/watch?v=j1-OyLo77ss
// https://www.youtube.com/watch?v=CSeUMTaNFYk

public class PatrollingGuardScript : MonoBehaviour
{
    public float movementSpeed = 2f; // Stores the movement speed
    public GameObject[] waypointArray; // Stores the waypoints the guard will move through.
    public PlayerControl player; // Stores the player control script.
    public GameObject playerCharacter; // Stores the player character game object.
    public Transform playerTransform; // Stores the player character transform component.
    public float detectionRadius = 5f; // Stores the detection radius
    public float detectionAngle = 90f; // Stores the detection angle
    public float distanceThreshold = 0.2f; // Stores the distance threshold for the waypoints
    public SpriteRenderer spriteRenderer; // Stores the sprite renderer component for the guard
    public Animator animator; // Stores the animator component for the guard
    public GameObject spottedNotice; // Stores the pop-up object, used to indicate the guard has spotted the player

    private int index = 0; // Index for the waypoint array


    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GameObject.FindGameObjectWithTag("Player"); // Gets the player character game object and stores it
        player = playerCharacter.GetComponent<PlayerControl>(); // Gets the player control script and stores it
        playerTransform = playerCharacter.transform; // Gets the transform component for the player character and stores it
        spriteRenderer = GetComponent<SpriteRenderer>(); // Gets the sprite renderer for the guard
        animator = GetComponent<Animator>(); // Gets the animator for the guard
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 direction = (waypointArray[index].transform.position - transform.position).normalized; // Calculates the vector for the direction between where the guard is currently standing, and where the next waypoint they need to patroll is. This vector is then normalized.

        // The next 3 lines are used to set the variables for the animator to display the correct walking animation.
        animator.SetFloat("Horizontal", direction.x);
        animator.SetFloat("Vertical", direction.y);
        animator.SetFloat("Speed", direction.sqrMagnitude);

        transform.Translate(movementSpeed * Time.deltaTime * direction); // Used to move the guard towards the next waypoint. This was taken from: https://medium.com/nerd-for-tech/player-movement-in-unity-2d-using-rigidbody2d-4f6f1693d730


        Vector2 directionToPlayer = playerTransform.position - transform.position; // Finds the direction to the player.
        float distanceToPlayer = directionToPlayer.magnitude; // Finds the distance to the player.
        float angleToPlayer = Vector2.Angle(direction, directionToPlayer); //Finds the angle to the player, based on which direction the guard is facing, and the direction the player.

        // If the distance to the player is less than or equals to the detection radius of the guard, and the angle is within the detection angle / 2, checks if the player is visible.
        if (distanceToPlayer <= detectionRadius && angleToPlayer <= detectionAngle / 2f)
        {
            // If the player is standing in light, the game is over, and the level restarts.
            if (player.visible)
            {
                player.spotted = true; // Spotted is set to true, to stop the player character from moving.
                spottedNotice.SetActive(true); // The pop-up above the guard becomes visible.
                Invoke(nameof(RestartLevel), 1); // Invokes the RestartLevel method after one second to give the player time to see where they messed up.
            }
        }

        // Used to make sure the sprite is facing the correct direction when moving horizontaly.
        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;

        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }

        // If the guard has reached the waypoint, the index increments and the guard will move to the next waypoint in the array.
        if (Vector2.Distance(transform.position, waypointArray[index].transform.position) <= distanceThreshold)
        {
            index++;

            // If the end of the array has been reached, the index resets to 0, and it loops.
            if (index >= waypointArray.Length)
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
            Invoke(nameof(RestartLevel), 1); // Inovkes the RestartLevel method after 1 second.
        }
    }


    // Loads the current level.
    public void RestartLevel()
    {
        player.spotted = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
    }


    // Draws a sphere for the detection radius in the Unity editor.
    // Draws the waypoints in the Unity editor.
    // Still couldn't figure out how to draw the correct angle.
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        for (int i = 0; i < waypointArray.Length; i++)
        {
            Gizmos.DrawSphere(waypointArray[i].transform.position, distanceThreshold);
            if (i < waypointArray.Length - 1)
            {
                Gizmos.DrawLine(waypointArray[i].transform.position, waypointArray[i + 1].transform.position);
            }
            else
            {
                Gizmos.DrawLine(waypointArray[i].transform.position, waypointArray[0].transform.position);
            }
        }

        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
