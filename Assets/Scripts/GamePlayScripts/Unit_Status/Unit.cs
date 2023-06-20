using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class Unit
{
    public Attribute HP { get; set; }
    public Attribute DMG { get; set; }
    public bool IsMelee { get; set; }
    public UNIT_TYPE UnitTYPE { get; set; } // unit type ( player or enemy or whatever ). ( like teams i guess).
    public Stopwatch DamageTakenTimer { get; set; }
    public float DamageTakenMill { get; set; }
    public Stopwatch AttackRateTimer { get; set; }
    public Attribute AttackRateMill { get; set; } 
    public Attribute Speed { get; set; }
    public int Level { get; set; } = 1;
    public float XP { get; set; } = 0;
    public float XpToLevelUp { get; set; } = 100;
    public List<Attribute> AttList { get; set; }

    private static readonly float _lvlUpMod = 1.15f;
    public Unit(Attribute hp , Attribute dmg , bool isMelee , UNIT_TYPE unitType , Attribute speed 
        , Attribute attackRateMill , float damageTakenMill)
    {
        HP = new Attribute(hp);
        DMG = new Attribute(dmg);
        IsMelee = isMelee;
        UnitTYPE = unitType;
        AttackRateMill = new Attribute(attackRateMill);
        DamageTakenMill = damageTakenMill;
        Speed = new Attribute(speed);
        DamageTakenTimer = new Stopwatch();
        AttackRateTimer = new Stopwatch();
        AttList = new List<Attribute>
        {
            HP,
            DMG,
            AttackRateMill,
            Speed,
        };
    }

    public Unit(Unit other)
    {
        this.HP = new Attribute(other.HP);
        this.DMG = new Attribute(other.DMG);
        this.IsMelee = other.IsMelee;
        this.UnitTYPE = other.UnitTYPE;
        this.Speed = new Attribute(other.Speed);
        this.AttackRateMill = new Attribute(other.AttackRateMill);
        this.DamageTakenMill = other.DamageTakenMill;
        DamageTakenTimer = new Stopwatch();
        AttackRateTimer = new Stopwatch();
        AttList = new List<Attribute>
        {
            HP,
            DMG,
            AttackRateMill,
            Speed,
        };
    }

    public bool IsDead()
    {
        return HP.Value <= 0;
    }

    public void AddXP(float amount)
    {
        XP += amount;
        if(XP >= XpToLevelUp)
        {
            LevelUp();
        }
    }

    // for now upgrades 1 random stat.
    public void LevelUp()
    {
        XP -= XpToLevelUp;
        Level++;
        XpToLevelUp *= _lvlUpMod;
        AttList[Random.Range(0, AttList.Count)].Upgrade();
    }


    public static readonly Dictionary<UNIT_TAG, Unit> UnitGiverDic = new Dictionary<UNIT_TAG, Unit>
    {
        {
            UNIT_TAG.Player_Default,
            new Unit
                (new Attribute("HP" , 100 , 10) 
                , new Attribute("DMG" , 10 , 1.5f)
                , false 
                , UNIT_TYPE.PLAYER 
                , new Attribute("SPEED" , 5 , 0.2f)
                , new Attribute("Attack Rate" , 200 , -10)
                , 1000)
        },

        {
            UNIT_TAG.Enemy_Default,
            new Unit
                (new Attribute("HP" , 25 , 10)
                , new Attribute("DMG" , 1 , 1)
                , true
                , UNIT_TYPE.ENEMY
                , new Attribute("SPEED" , 5 , 0.2f)
                , new Attribute("Attack Rate" , 1000 , -10)
                , 50)
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

public class Attribute
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float ValPerLvl { get; set; }

    public Attribute(string name , float value , float valPerLvl)
    {
        Name = name;
        Value = value;
        ValPerLvl = valPerLvl;
    }

    public Attribute(Attribute other)
    {
        this.Name = other.Name;
        this.Value = other.Value;
        this.ValPerLvl = other.ValPerLvl;
    }

    public void Upgrade()
    {
        Value += ValPerLvl;
    }
}
