using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This script updates the camera location to follow the player character

public class CameraScript : MonoBehaviour
{

    public Transform player; //This stores the player character Transform data, which will be used to update the camera location

    // Update is called once per frame
    // The code here was taken from https://generalistprogrammer.com/unity/unity-2d-how-to-make-camera-follow-player/
    void Update()
    {
        // Every frame, the camera position is updated to the player character position, with the z position unchanged
        this.transform.position = new Vector3(player.position.x, player.position.y, this.transform.position.z);
    }
}
