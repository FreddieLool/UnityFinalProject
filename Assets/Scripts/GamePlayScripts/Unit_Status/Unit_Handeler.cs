using UnityEngine;
using System.Diagnostics;

public class Unit_Handeler : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag; // general unit tag ( diff for each unit )
    [SerializeField] RewardedAdsButton RewardedAdsButton;

    public Unit unit;
    private Unit _collidedUnit;
    private GameObject _unitCanvas;
    private Stopwatch _soundTimer = new Stopwatch();
  
    //for now :
    public int EnemiesKilled;
    private float _deadUnitXp = 25;
    private float _totalMill = MapProcGen.GlobalTotalMill + MapProcGen.EnemyXpGainMill;
    private static readonly int _enemyModifiedChance = 17;
    private float _soundMill = 950;



    private void Awake()
    {
        unit = new Unit(Unit.UnitGiverDic[UnitTag]);
        unit.UnitTag = this.UnitTag;
    }

    void Start()
    {
        unit.ImmortalTimer.Start();
        unit.RegenTimer.Start();
        if (unit.UnitType == UNIT_TYPE.ENEMY)
        {
            _unitCanvas = GameObject.Find("GameTextCanvas");

            Instantiate(Resources.Load("Prefabs/Units/HealthBarBackground", typeof(GameObject)) as GameObject
            , this.transform.position + new Vector3(0, 1.15f, 0)
            , Quaternion.identity, _unitCanvas.transform).GetComponent<UnitMapHpBar>()
            .ActivateHpBar(this.gameObject, unit);

            if (Random.Range(1, 100) <= _enemyModifiedChance)
            {
                unit.UnitMod = Unit.UmList[Random.Range(0, Unit.UmList.Count)];
            }
            unit.AddXP(MapProcGen.TotalEnemyXpGain);
            gameObject.AddComponent<EnemyPathFinding>();
        }



        unit.UpdateUnitByModifier(unit.UnitMod);
        gameObject.GetComponent<SpriteRenderer>().color = unit.UnitColor;


    }


    private void FixedUpdate()
    {
        if (GameOver.IsGamePaused) { return; }

        if (unit.ImmortalTimer.ElapsedMilliseconds >= unit.ImmortalMill) { unit.ImmortalTimer.Reset(); }

        if (unit.RegenTimer.ElapsedMilliseconds >= unit.RegenMill) { unit.RegenTimer.Restart(); unit.HP.Value += unit.HpRegen.Value; }

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
        if (GameOver.IsGamePaused) { Destroy(gameObject); }
        Combat(collision);
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GameOver.IsGamePaused) { Destroy(gameObject); }
        Combat(collision);
    }

    private void Combat(Collision2D collision)
    {
        _collidedUnit = null;

        if (unit == null)
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
            TakeDmg();
            unit.ImmortalTimer.Start();
        }
    }

    private void Die()
    {
        _collidedUnit.AddXP(_deadUnitXp);
        ScorePlayer.AddScore(SCORE_TYPE.KILL);
        if (unit.UnitType == UNIT_TYPE.ENEMY)
        {
            EnemiesKilled++;
            Destroy(gameObject);
        }
        else if (unit.UnitType == UNIT_TYPE.PLAYER)
        {
            this.gameObject.SetActive(false);
            RewardedAdsButton.LoadAd();
        }
    }
    private void TakeDmg()
    {
        float enemyDmg = _collidedUnit.DealDamage();
        unit.HP.Value -= enemyDmg;
        AddDamageDealtByUnitText(enemyDmg);
        if (unit.UnitType == UNIT_TYPE.ENEMY && (!_soundTimer.IsRunning || _soundTimer.ElapsedMilliseconds >= _soundMill))
        {
            GameObject usGO = Resources.Load("Prefabs/Units/UnitSoundGO", typeof(GameObject)) as GameObject;
            Instantiate(usGO, this.transform.position, this.transform.rotation)
                .GetComponent<UnitSoundScript>()
                .SetUnit(this.gameObject);

            _soundTimer.Restart();
        }

        if (unit.IsDead())
        {
            Die();
        }
    }

    public void AddDamageDealtByUnitText(float damage)
    {
        Instantiate(Resources.Load("Prefabs/Units/DamageDealtByUnit", typeof(GameObject)) as GameObject
            , this.transform.position + new Vector3(0, 1.80f, 0)
            , Quaternion.identity).GetComponent<DamageDeltByUnitText>()
            .ActivateDamageText(damage, this.gameObject, _collidedUnit.AttackRateMill.Value, _collidedUnit.DidCrit);
    }

    public void ApplyAdRevive()
    {
        unit.HP.Value = 100 + (unit.HP.ValPerLvl * unit.HP.AttLvl);
    }
}



