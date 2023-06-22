using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Linq;
using System;
public class Unit
{
    public static List<UNIT_MODIFIER> UmList = Enum.GetValues(typeof(UNIT_MODIFIER)).Cast<UNIT_MODIFIER>().ToList();

    public Attribute HP { get; set; }
    public Attribute DMG { get; set; }
    public bool IsMelee { get; set; }
    public UNIT_TYPE UnitType { get; set; } // unit type ( player or enemy or whatever ). ( like teams i guess).
    public Stopwatch ImmortalTimer { get; set; }
    public float ImmortalMill { get; set; }
    public Stopwatch AttackRateTimer { get; set; }
    public Attribute AttackRateMill { get; set; } 
    public Attribute Speed { get; set; }
    public int Level { get; set; } = 1;
    public float XP { get; set; } = 0;
    public float XpToLevelUp { get; set; } = 100;
    public List<Attribute> AttList { get; set; }
    public UNIT_MODIFIER UnitMod { get; set; } = UNIT_MODIFIER.Normal; // default modifier.
    public UNIT_TAG UnitTag { get; set; }
    public Color UnitColor { get; set; } = Color.white; // default color.

    private static readonly float _lvlUpMod = 1.15f;
    public Unit(Attribute hp , Attribute dmg , bool isMelee , UNIT_TYPE unitType , Attribute speed 
        , Attribute attackRateMill , float damageTakenMill)
    {
        HP = new Attribute(hp);
        DMG = new Attribute(dmg);
        IsMelee = isMelee;
        UnitType = unitType;
        AttackRateMill = new Attribute(attackRateMill);
        ImmortalMill = damageTakenMill;
        Speed = new Attribute(speed);
        ImmortalTimer = new Stopwatch();
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
        this.UnitType = other.UnitType;
        this.Speed = new Attribute(other.Speed);
        this.AttackRateMill = new Attribute(other.AttackRateMill);
        this.ImmortalMill = other.ImmortalMill;
        ImmortalTimer = new Stopwatch();
        AttackRateTimer = new Stopwatch();
        AttList = new List<Attribute> //DO not change the order!!!
        {
            HP, /* 0 */
            DMG, /* 1 */
            AttackRateMill, /* 2 */
            Speed, /* 3 */
        };
    }

    public bool IsDead()
    {
        return HP.Value <= 0;
    }

    public void AddXP(float amount)
    {
        XP += amount;
        while(XP >= XpToLevelUp)
        {
            LevelUp();
        }
    }

    // for now (if player upgrades 1 random stat) ( if enemy upgrades all stats a bit)
    public void LevelUp()
    {
        XP -= XpToLevelUp;
        Level++;
        XpToLevelUp *= _lvlUpMod;
        if(UnitType == UNIT_TYPE.PLAYER)
        {
            AttList[UnityEngine.Random.Range(0, AttList.Count)].AttLvlUpUpgrade();
        }
        if (UnitType == UNIT_TYPE.ENEMY)
        {
            EnemyLvlUp();
        }
    }


    public static readonly Dictionary<UNIT_TAG, Unit> UnitGiverDic = new Dictionary<UNIT_TAG, Unit>
    {
        {
            UNIT_TAG.PLAYER_DEFAULT,
            new Unit
                (
                new Attribute("HP" , 100 , 15 , 0) 
                , new Attribute("DMG" , 10 , 2.5f , 0)
                , false 
                , UNIT_TYPE.PLAYER 
                , new Attribute("SPEED" , 5 , 0.4f , 30)
                , new Attribute("Attack Rate" , 200 , -10 , 12)
                , 500
                )
        },

        {
            UNIT_TAG.ENEMY_DEFAULT,
            new Unit
                (
                new Attribute("HP" , 25 , 2.5f , 0 )
                , new Attribute("DMG" , 3 , 0.5f , 0)
                , true
                , UNIT_TYPE.ENEMY
                , new Attribute("SPEED" , 5 , 0.05f , 35)
                , new Attribute("Attack Rate" , 1000 , -10 , 75)
                , 50
                )
        },
    };

    public void EnemyLvlUp(int amount = 1)
    {
        for (int i = 0; i < AttList.Count; i++)
        {
            for (int j = 0; j < amount; j++)
            {
                AttList[i].AttLvlUpUpgrade();
            }
        }
    }

    public void UpdateUnitByModifier(UNIT_MODIFIER um)
    {
        switch (um)
        {
            case UNIT_MODIFIER.TANKY:
                HP.Value *= 3;
                UnitColor = Color.red;
                break;

            case UNIT_MODIFIER.DMG_DEALER:
                DMG.Value *= 3;
                UnitColor = Color.cyan;
                break;

            case UNIT_MODIFIER.FAST_ATTACKER:
                AttackRateMill.Value /= 2;
                UnitColor = Color.blue;
                break;

            case UNIT_MODIFIER.SPEEDY:
                Speed.Value *= 1.5f;
                UnitColor = Color.black;
                break;

            default:
                break;
        }
    }
}

public enum UNIT_TAG
{
    ENEMY_DEFAULT,
    PLAYER_DEFAULT,
}

public enum UNIT_TYPE
{
    PLAYER,
    ENEMY,
}

public enum UNIT_MODIFIER
{
    Normal,
    TANKY,
    SPEEDY,
    FAST_ATTACKER,
    DMG_DEALER,
}


public class Attribute
{
    public string Name { get; set; }
    public float Value { get; set; }
    public float ValPerLvl { get; set; }
    public int AttLvl { get; set; } = 0;
    public int LvlCap { get; set; } // if lvlcap = 0 , there is no level cap!

    public Attribute(string name , float value , float valPerLvl , int lvlcap)
    {
        Name = name;
        Value = value;
        ValPerLvl = valPerLvl;
        LvlCap = lvlcap;
    }

    public Attribute(Attribute other)
    {
        this.Name = other.Name;
        this.Value = other.Value;
        this.ValPerLvl = other.ValPerLvl;
    }

    // upgrading after lvl up
    public void AttLvlUpUpgrade()
    {
        if(LvlCap == 0 || AttLvl <= LvlCap)
        {
            Value += ValPerLvl;
            AttLvl++;
        }
    }

    // upgrade with custom val
    public void Upgrade(float addedVal)
    {
        Value += addedVal;
    }
}
