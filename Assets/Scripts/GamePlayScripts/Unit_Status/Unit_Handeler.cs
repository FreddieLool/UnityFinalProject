using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Unit_Handeler : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag;
    public Unit unit;
    public float UHP;
    public float UDMG;

    private Stopwatch _dmgTakenTimer = new Stopwatch();
    private float _dmgTakenMill;



    void Start()
    {
        unit = new Unit(Unit.UnitGiverDic[UnitTag]);
        if (UnitTag == UNIT_TAG.Player_Default) { _dmgTakenMill = 1000; }
        else { _dmgTakenMill = 30; }

        _dmgTakenTimer.Start();
    }
    private void FixedUpdate()
    {
        UHP = unit.HP;
        UDMG = unit.DMG;
        if(_dmgTakenTimer.ElapsedMilliseconds >= _dmgTakenMill)
        {
            _dmgTakenTimer.Reset();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Unit collidedObj;
      
        if(!_dmgTakenTimer.IsRunning)
        {
            if (collision.gameObject.GetComponent<Unit_Handeler>() != null)
            {
                if (collision.gameObject.GetComponent<Unit_Handeler>().unit.IsMelee)
                {
                    collidedObj = new Unit(collision.gameObject.GetComponent<Unit_Handeler>().unit);
                    TakeDmg(collidedObj.DMG);
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Bullet>() != null)
                {
                    collidedObj = collision.gameObject.GetComponent<Bullet>().Owner.GetComponent<Unit_Handeler>().unit;
                    TakeDmg(collidedObj.DMG);
                }
            }
            _dmgTakenTimer.Start();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }

    public void TakeDmg(float enemyDmg)
    {
        unit.HP -= enemyDmg;
        if (unit.HP <= 0) { Die(); }
    }


}
