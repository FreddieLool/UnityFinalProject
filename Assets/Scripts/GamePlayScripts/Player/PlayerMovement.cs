using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // public stuff :
    public float Speed = 5;
    public Rigidbody2D rb;
    public Camera Cam;
    // private vectors to get pos:
    Vector2 _movement;
    Vector2 _mousePos;

    void Update()
    {
        // setting up basic movement :
        _movement.x = Input.GetAxisRaw("Horizontal");
        _movement.y = Input.GetAxisRaw("Vertical");

        // getting current mouse pos :
        _mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);
    }

    private void FixedUpdate()
    {
        // movement and angle updates once per frame here :
        rb.MovePosition(rb.position + _movement * Speed * Time.fixedDeltaTime);

        // getting and applying the angle with mouse pos so the player model can look where the mouse is :
        // CONVERT TO JOYSTICK LATER !!
        Vector2 lookDir = _mousePos - rb.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90;
        rb.rotation = angle;
    }
}
