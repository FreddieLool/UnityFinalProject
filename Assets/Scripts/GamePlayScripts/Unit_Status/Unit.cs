using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class Unit
{
    public float HP { get; set; }
    public float DMG { get; set; }
    public bool IsMelee { get; set; }
    public UNIT_TYPE UnitTYPE { get; set; } // unit type ( player or enemy or whatever ). ( like teams i guess).
    public Stopwatch DamageTakenTimer { get; set; }
    public float DamageTakenMill { get; set; }
    public Stopwatch AttackRateTimer { get; set; }
    public float AttackRateMill { get; set; }
    public float Speed { get; set; }

    public Unit(float hp ,  float dmg , bool isMelee , UNIT_TYPE unitType , float speed 
        , float attackRateMill , float damageTakenMill)
    {
        HP = hp;
        DMG = dmg;
        IsMelee = isMelee;
        UnitTYPE = unitType;
        AttackRateMill = attackRateMill;
        DamageTakenMill = damageTakenMill;
        Speed = speed;
        DamageTakenTimer = new Stopwatch();
        AttackRateTimer = new Stopwatch();
    }

    public Unit(Unit other)
    {
        this.HP = other.HP;
        this.DMG = other.DMG;
        this.IsMelee = other.IsMelee;
        this.UnitTYPE = other.UnitTYPE;
        this.Speed = other.Speed;
        this.AttackRateMill = other.AttackRateMill;
        this.DamageTakenMill = other.DamageTakenMill;
        DamageTakenTimer = new Stopwatch();
        AttackRateTimer = new Stopwatch();
    }

    public static readonly Dictionary<UNIT_TAG, Unit> UnitGiverDic = new Dictionary<UNIT_TAG, Unit>
    {
        {
            UNIT_TAG.Player_Default,
            new Unit(1000 , 10 , false , UNIT_TYPE.PLAYER , 5 , 200 , 1000)
        },

        {
            UNIT_TAG.Enemy_Default,
            new Unit(25 , 1 , true , UNIT_TYPE.ENEMY , 5 , 500 , 50)
        },
    };
}

public enum UNIT_TAG
{
    Enemy_Default,
    Player_Default,
}

public enum UNIT_TYPE
{
    PLAYER,
    ENEMY,
}