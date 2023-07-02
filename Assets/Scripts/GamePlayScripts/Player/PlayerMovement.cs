using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //private
    private Unit _playerUnit;
    private float _rotationSpeed = 590; // the speed of the rotation.
    private float _speed;
    private UnityStandardAssets.CrossPlatformInput.Joystick _joystick;
    // private vectors to get pos:
    Vector2 _movement;
    Vector2 _lookDir;
    Vector2 _mousePos;


    // SerializeField stuff :
    [SerializeField] GameObject MovingJoystickGO;
    [SerializeField] GameObject ShootingJoystickGO;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Camera Cam;


    private void Start()
    {
        _playerUnit = GetComponent<Unit_Handeler>().Unit;
        _speed = _playerUnit.Speed.Value;
    }

    void Update()
    {
        if (GameOver.IsGamePaused) { return; }

        if (!PlayerUI.JOYSTICK_MODE)
        {
            // setting up basic movement :
            _movement.x = Input.GetAxisRaw("Horizontal");
            _movement.y = Input.GetAxisRaw("Vertical");

            // getting current mouse pos :
            _mousePos = Cam.ScreenToWorldPoint(Input.mousePosition);

            _lookDir = _mousePos - rb.position;
        }
        if (PlayerUI.JOYSTICK_MODE)
        {
            _movement = MovingJoystickGO.transform.localPosition.normalized;

            _lookDir = ShootingJoystickGO.transform.localPosition.normalized;
        }
    }

    private void FixedUpdate()
    {
        if (GameOver.IsGamePaused) { return; }

        // movement and angle updates once per frame here :
        rb.MovePosition(rb.position + _movement * _speed * Time.fixedDeltaTime);

        // getting and applying the angle with mouse pos so the player model can look where the mouse is :
        if(ShootingJoystickGO.transform.localPosition == Vector3.zero) { return; }

        float trgetAngle = Mathf.Atan2(_lookDir.y, _lookDir.x) * Mathf.Rad2Deg;

        rb.rotation = Mathf.MoveTowardsAngle(rb.rotation, trgetAngle, _rotationSpeed * Time.deltaTime);
    }
}
