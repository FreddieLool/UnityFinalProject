using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Unit_Handeler : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag; // general unit tag ( diff for each unit )

    public Unit unit;
    private Unit _collidedUnit;

    //for now :
    private float _deadUnitXp = 15;

    void Start()
    {
        unit = new Unit(Unit.UnitGiverDic[UnitTag]);
        unit.DamageTakenTimer.Start();
    }

    private void FixedUpdate()
    {
        if(unit.DamageTakenTimer.ElapsedMilliseconds >= unit.DamageTakenMill)
        {
            unit.DamageTakenTimer.Reset();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Combat(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        Combat(collision);
    }

    private void Combat(Collision2D collision)
    {
        _collidedUnit = null;

        if(unit.DamageTakenTimer == null)
        {
            return;
        }
        if (!unit.DamageTakenTimer.IsRunning)
        {
            if (collision.gameObject.GetComponent<Unit_Handeler>() != null)
            {
                Unit_Handeler collidedUnitHandeler = collision.gameObject.GetComponent<Unit_Handeler>();

                if (collidedUnitHandeler.unit.IsMelee)
                {
                    _collidedUnit = collidedUnitHandeler.unit;
                }
            }
            else
            {
                if (collision.gameObject.GetComponent<Bullet>() != null)
                {
                    _collidedUnit = collision.gameObject.GetComponent<Bullet>()
                        .Owner.GetComponent<Unit_Handeler>().unit;
                }
            }

            if (_collidedUnit != null && unit.UnitTYPE != _collidedUnit.UnitTYPE)
            {
                TakeDmg(_collidedUnit.DMG.Value);
                unit.DamageTakenTimer.Start();
            }
        }
    }

    private void Die()
    {
        _collidedUnit.AddXP(_deadUnitXp);
        Destroy(gameObject);
    }

    public void TakeDmg(float enemyDmg)
    {
        unit.HP.Value -= enemyDmg;
        if (unit.IsDead()) { Die(); }
    }

}



