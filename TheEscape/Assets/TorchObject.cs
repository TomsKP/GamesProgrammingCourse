using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;


// This script is responsible for the torch functionality.
public class TorchObject : MonoBehaviour
{

    private Transform lightObject; // Stores the transform component for the torch
    private new Light2D light; // Stores the Light2D component
    private CircleCollider2D circleCollider; // Stores the CircleCollider component

    private bool nearObject; // Used to check if the player is close enough to interact

    private void Start()
    {
        lightObject = this.gameObject.transform.GetChild(0); // Gets the child object for the torch and stores it
        light = lightObject.GetComponent<Light2D>(); // Gets the Light2D component and stores it
        circleCollider = lightObject.GetComponent<CircleCollider2D>(); // Gets the CircleCollider component and stores it
    }

    private void Update()
    {
        // Checks if the player is close enough to the torch to interact, and if they are, if they press "F", the torch will extinguish.
        if(Input.GetKey(KeyCode.F) && nearObject)
        {
            light.intensity = 0; // Sets the torch light intesity to 0. 
            circleCollider.enabled = false; // Disables the collider. This makes it so the player is no longer visible by the guards.
        }
        
    }

    // If the player enters the trigger, they are close enought to interact with the torch.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            nearObject = true;
        }
    }

    // Leaving the trigger makes the player unable to interact with the torch.
    private void OnTriggerExit2D(Collider2D other)
    {
        nearObject = false;
    }
}
