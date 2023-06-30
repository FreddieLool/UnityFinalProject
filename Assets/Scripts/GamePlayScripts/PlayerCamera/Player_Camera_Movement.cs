using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Camera_Movement : MonoBehaviour
{
    public Transform Player;
    public float SmoothSpeed = 0.1f;

    // IMPORTENT TO HAVE IT BE FixedUpdate !!
    private void FixedUpdate()
    {
        if (GameOver.IsGamePaused) { return; }

        // applying "smooth movement" and making the cam follow the player :
        Vector3 desiredPos = new Vector3(Player.position.x , Player.position.y , transform.position.z);
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, SmoothSpeed);
        transform.position = smoothedPos;
    }
}
