using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    private Unit _unit = null;

    private Rigidbody2D _rb2D;

    private GameObject _player;


    // applying player ( not the prefab ) to the private player gameObject :
    private void Start()
    {
        _rb2D = gameObject.GetComponent<Rigidbody2D>();
        _player = GameObject.Find("Player");
        _unit = gameObject.GetComponent<Unit_Handeler>().unit;
    }

    // making  the enemy ai follow the player and change his angle that he will look at the player also :
    private void FixedUpdate()
    {
        Vector2 direction = _player.transform.position - this.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg;

        //making ai move twards the player :
        _rb2D.MovePosition(_rb2D.position + direction * _unit.Speed.Value * Time.fixedDeltaTime);

        // making the ai look at the player:
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
