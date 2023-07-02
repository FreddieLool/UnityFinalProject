using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class EnemyPathFinding : MonoBehaviour
{
    private Unit _unit = null;

    private Rigidbody2D _rb2D;

    private GameObject _player;

    private Stopwatch _livingTimer = new Stopwatch();

    private static readonly float _livingDuration = 15000;
    private static readonly float _livingAllowedDistance = 35;

    // applying player ( not the prefab ) to the private player gameObject :
    private void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");
        _unit = gameObject.GetComponent<Unit_Handeler>().Unit;
        _livingTimer.Start();
    }

    // making  the enemy ai follow the player and change his angle that he will look at the player also :
    private void FixedUpdate()
    {
        if (GameOver.IsGamePaused) { return; }

        // makes enemies despawn when they are alive for a long time +
        // are far away from the player to reduce lag.
        if(_livingTimer.ElapsedMilliseconds >= _livingDuration)
        {
            float distance = Vector2.Distance(_player.transform.position , this.transform.position);
            if(distance >= _livingAllowedDistance)
            {
                Destroy(gameObject);
            }
        }
        

        Vector2 direction = _player.transform.position - this.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg;

        //making ai move twards the player :
        _rb2D.MovePosition(_rb2D.position + direction * _unit.Speed.Value * Time.fixedDeltaTime);

        // making the ai look at the player:
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
