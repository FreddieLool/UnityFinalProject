using UnityEngine;
using System.Diagnostics;

public class Unit_Handeler : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag; // general unit tag ( diff for each unit )
    [SerializeField] RewardedAdsButton RewardedAdsButton;

    public Unit Unit;
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
        Unit = new Unit(Unit.UnitGiverDic[UnitTag]);
        Unit.UnitTag = this.UnitTag;
    }

    void Start()
    {
        Unit.ImmortalTimer.Start();
        Unit.RegenTimer.Start();
        if (Unit.UnitType == UNIT_TYPE.ENEMY)
        {
            _unitCanvas = GameObject.Find("GameTextCanvas");

            Instantiate(Resources.Load("Prefabs/Units/HealthBarBackground", typeof(GameObject)) as GameObject
            , this.transform.position + new Vector3(0, 1.15f, 0)
            , Quaternion.identity, _unitCanvas.transform).GetComponent<UnitMapHpBar>()
            .ActivateHpBar(this.gameObject, Unit);

            if (Random.Range(1, 100) <= _enemyModifiedChance)
            {
                Unit.UnitMod = Unit.UmList[Random.Range(0, Unit.UmList.Count)];
            }
            Unit.AddXP(MapProcGen.TotalEnemyXpGain);
            gameObject.AddComponent<EnemyPathFinding>();
        }



        Unit.UpdateUnitByModifier(Unit.UnitMod);
        gameObject.GetComponent<SpriteRenderer>().color = Unit.UnitColor;


    }


    private void FixedUpdate()
    {
        if (GameOver.IsGamePaused) { return; }

        if (Unit.ImmortalTimer.ElapsedMilliseconds >= Unit.ImmortalMill) { Unit.ImmortalTimer.Reset(); }

        if (Unit.RegenTimer.ElapsedMilliseconds >= Unit.RegenMill) { Unit.RegenTimer.Restart(); Unit.HP.Value += Unit.HpRegen.Value; }

        if (Unit.UnitType == UNIT_TYPE.ENEMY)
        {
            if (MapProcGen.EnemyXpGainTimer.ElapsedMilliseconds >= _totalMill)
            {
                _totalMill += MapProcGen.EnemyXpGainMill;
                Unit.AddXP(MapProcGen.EnemyXpGain);
            }
        }
        //if (Unit.UnitType == UNIT_TYPE.PLAYER)
        //{
        //    Unit.AddXP(5);
        //}
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

        if (Unit == null)
        {
            return;
        }
        if (collision.gameObject.GetComponent<Unit_Handeler>() != null)
        {
            Unit_Handeler collidedUnitHandeler = collision.gameObject.GetComponent<Unit_Handeler>();

            if (collision.gameObject.GetComponent<Unit_Handeler>().Unit != null &&
                collidedUnitHandeler.Unit.IsMelee)
            {
                _collidedUnit = collidedUnitHandeler.Unit;
            }
        }
        else
        {
            if (collision.gameObject.GetComponent<Bullet>() != null)
            {
                _collidedUnit = collision.gameObject.GetComponent<Bullet>()
                    .Owner.GetComponent<Unit_Handeler>().Unit;
            }
        }

        if (_collidedUnit != null
            && !Unit.ImmortalTimer.IsRunning
            && Unit.UnitType != _collidedUnit.UnitType)
        {
            TakeDmg();
            Unit.ImmortalTimer.Start();
        }
    }

    private void Die()
    {
        _collidedUnit.AddXP(_deadUnitXp);
        ScorePlayer.AddScore(SCORE_TYPE.KILL);
        if (Unit.UnitType == UNIT_TYPE.ENEMY)
        {
            EnemiesKilled++;
            Destroy(gameObject);
        }
        else if (Unit.UnitType == UNIT_TYPE.PLAYER)
        {
            RewardedAdsButton.LoadAd();
            this.gameObject.SetActive(false);
        }
    }
    private void TakeDmg()
    {
        float enemyDmg = _collidedUnit.DealDamage();
        Unit.HP.Value -= enemyDmg;
        AddDamageDealtByUnitText(enemyDmg);
        if (Unit.UnitType == UNIT_TYPE.ENEMY && (!_soundTimer.IsRunning || _soundTimer.ElapsedMilliseconds >= _soundMill))
        {
            GameObject usGO = Resources.Load("Prefabs/Units/UnitSoundGO", typeof(GameObject)) as GameObject;
            Instantiate(usGO, this.transform.position, this.transform.rotation)
                .GetComponent<UnitSoundScript>()
                .SetUnit(this.gameObject);

            _soundTimer.Restart();
        }

        if (Unit.IsDead())
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
        Unit.HP.Value = 100 + (Unit.HP.ValPerLvl * Unit.HP.AttLvl);
        GameOver.ResumeGame();
    }
}



