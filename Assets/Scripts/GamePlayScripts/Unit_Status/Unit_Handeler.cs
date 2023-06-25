using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
using System.Linq;
using UnityEngine.Advertisements;


public class Unit_Handeler : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag; // general unit tag ( diff for each unit )
    [SerializeField] RewardedAdsButton RewardedAdsButton;

    public Unit unit;
    private Unit _collidedUnit;
    private GameObject _unitCanvas;
    //for now :
    private float _deadUnitXp = 25;
    private float _totalMill = MapProcGen.GlobalTotalMill + MapProcGen.EnemyXpGainMill;
    private static readonly int _enemyModifiedChance = 17;

    private void Awake()
    {
        unit = new Unit(Unit.UnitGiverDic[UnitTag]);
        unit.UnitTag = this.UnitTag;
    }

    void Start()
    {
        unit.ImmortalTimer.Start();

        if (unit.UnitType == UNIT_TYPE.ENEMY)
        {
            _unitCanvas = GameObject.Find("UnitCanvas");

            Instantiate(Resources.Load("Prefabs/Units/HealthBarBackground", typeof(GameObject)) as GameObject
            , this.transform.position + new Vector3(0, 1.15f, 0)
            , Quaternion.identity, _unitCanvas.transform).GetComponent<UnitMapHpBar>()
            .ActivateHpBar(this.gameObject , unit);


            if (UnityEngine.Random.Range(1, 100) <= _enemyModifiedChance)
            {
                unit.UnitMod = Unit.UmList[UnityEngine.Random.Range(0, Unit.UmList.Count)];
            }
            unit.AddXP(MapProcGen.TotalEnemyXpGain);
            gameObject.AddComponent<EnemyPathFinding>();
        }

        unit.UpdateUnitByModifier(unit.UnitMod);
        gameObject.GetComponent<SpriteRenderer>().color = unit.UnitColor;


    }

    private void FixedUpdate()
    {
        if(unit.ImmortalTimer.ElapsedMilliseconds >= unit.ImmortalMill)
        {
            unit.ImmortalTimer.Reset();
        }

        if (unit.UnitType == UNIT_TYPE.ENEMY)
        {
            if (MapProcGen.EnemyXpGainTimer.ElapsedMilliseconds >= _totalMill)
            {
                _totalMill += MapProcGen.EnemyXpGainMill;
                unit.AddXP(MapProcGen.EnemyXpGain);
            }
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

        if(unit == null)
        {
            return;
        }
        if (collision.gameObject.GetComponent<Unit_Handeler>() != null)
        {
            Unit_Handeler collidedUnitHandeler = collision.gameObject.GetComponent<Unit_Handeler>();

            if (collision.gameObject.GetComponent<Unit_Handeler>().unit != null &&
                collidedUnitHandeler.unit.IsMelee)
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

        if (_collidedUnit != null 
            && !unit.ImmortalTimer.IsRunning
            && unit.UnitType != _collidedUnit.UnitType)
        {
            TakeDmg(_collidedUnit.DMG.Value);
            unit.ImmortalTimer.Start();
        }


    }

    private void Die()
    {
        _collidedUnit.AddXP(_deadUnitXp);
        ScorePlayer.AddScore(SCORE_TYPE.KILL);
        if(unit.UnitType == UNIT_TYPE.ENEMY)
        {
            Destroy(gameObject);
        }
        else if (unit.UnitType == UNIT_TYPE.PLAYER)
        {
            this.gameObject.SetActive(false);
            RewardedAdsButton.LoadAd();
        }
    }
    private void TakeDmg(float enemyDmg)
    {
        unit.HP.Value -= enemyDmg;
        AddDamageDealtByUnitText(enemyDmg);
        if (unit.IsDead()) { Die(); }
    }

    public void AddDamageDealtByUnitText(float damage)
    { 
        Instantiate(Resources.Load("Prefabs/Units/DamageDealtByUnit", typeof(GameObject)) as GameObject
            , this.transform.position + new Vector3(0, 1.80f, 0)
            , Quaternion.identity).GetComponent<DamageDeltByUnitText>()
            .ActivateDamageText(damage , this.gameObject , _collidedUnit.AttackRateMill.Value);
    }
}



