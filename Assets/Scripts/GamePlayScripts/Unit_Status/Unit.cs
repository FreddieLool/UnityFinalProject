using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit
{
    public float HP { get; set; }
    public float DMG { get; set; }
    public bool IsMelee { get; set; }

    public Unit(float hp ,  float dmg , bool isMelee)
    {
        HP = hp;
        DMG = dmg;
        IsMelee = isMelee;
    }

    public Unit(Unit other)
    {
        this.HP = other.HP;
        this.DMG = other.DMG;
        this.IsMelee = other.IsMelee;
    }

    public static readonly Dictionary<UNIT_TAG, Unit> UnitGiverDic = new Dictionary<UNIT_TAG, Unit>
    {
        {
            UNIT_TAG.Player_Default,
            new Unit(1000 , 10 , false)
        },

        {
            UNIT_TAG.Enemy_Default,
            new Unit(25 , 1 , true)
        },
    };
}

public enum UNIT_TAG
{
    Enemy_Default,
    Player_Default,
}
