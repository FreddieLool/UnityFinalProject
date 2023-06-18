using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public float HP { get; set; }
    public float DMG { get; set; }
    public bool IsMelee { get; set; }
    public UNIT_TYPE UnitTYPE { get; set; }// unit type ( player or enemy or whatever ). ( like teams i guess).

    public Unit(float hp ,  float dmg , bool isMelee , UNIT_TYPE unitType)
    {
        HP = hp;
        DMG = dmg;
        IsMelee = isMelee;
        UnitTYPE = unitType;
    }

    public Unit(Unit other)
    {
        this.HP = other.HP;
        this.DMG = other.DMG;
        this.IsMelee = other.IsMelee;
        this.UnitTYPE = other.UnitTYPE;
    }

    public static readonly Dictionary<UNIT_TAG, Unit> UnitGiverDic = new Dictionary<UNIT_TAG, Unit>
    {
        {
            UNIT_TAG.Player_Default,
            new Unit(1000 , 10 , false , UNIT_TYPE.PLAYER)
        },

        {
            UNIT_TAG.Enemy_Default,
            new Unit(25 , 1 , true , UNIT_TYPE.ENEMY)
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