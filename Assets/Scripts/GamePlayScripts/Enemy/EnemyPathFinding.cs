using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPathFinding : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag;
    private Unit _unit;

    [SerializeField] Rigidbody2D Rb2D;

    private GameObject _player;

    // applying player ( not the prefab ) to the private player gameObject :
    private void Start()
    {
        _unit = Unit.UnitGiverDic[UnitTag];
        _player = GameObject.Find("Player");
    }

    // making  the enemy ai follow the player and change his angle that he will look at the player also :
    private void FixedUpdate()
    {
        Vector2 direction = _player.transform.position - this.transform.position;
        direction.Normalize();
        float angle = Mathf.Atan2(direction.y , direction.x) * Mathf.Rad2Deg - 90;

        //making ai move twards the player :
        Rb2D.MovePosition(Rb2D.position + direction * _unit.Speed.Value * Time.fixedDeltaTime);


        // making the ai look at the player:
        transform.rotation = Quaternion.Euler(Vector3.forward * angle);
    }
}
