using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StatsPlayer : MonoBehaviour
{
    private
    float
        HP,
        DMG,
        AttackRate,
        Speed,
        Level,
        XP,
        XpTpLevelUp;

    private bool _once = false;

    private Unit _playerUnit;

    private TextMeshProUGUI _stats;

    void Start()
    {
        _stats = GetComponent<TextMeshProUGUI>();
    }

    
    private void FixedUpdate()
    {
        if(GameObject.Find("Player").GetComponent<Unit_Handeler>().unit == null) { return; }
        else if(!_once) 
        { 
            _once = true;
            _playerUnit = GameObject.Find("Player").GetComponent<Unit_Handeler>().unit;
        }
        HP = _playerUnit.HP.Value;
        DMG = _playerUnit.DMG.Value;
        AttackRate = _playerUnit.AttackRateMill.Value;
        Speed = _playerUnit.Speed.Value;
        XP = _playerUnit.XP;
        Level = _playerUnit.Level;
        XpTpLevelUp = _playerUnit.XpToLevelUp;
    }

    void Update()
    {
        if (!_once) { return; }

        _stats.text =
            "\n"+
            $"Health : {HP } \n\n"+
            $"Damage : {DMG } \n\n"+
            $"AttackRate : {AttackRate } \n\n"+
            $"Speed : {Speed }\n\n"+
            $"Level : {Level }\n\n"+
            $"XP : {(int)XP} / {(int)XpTpLevelUp}";
    }
}
