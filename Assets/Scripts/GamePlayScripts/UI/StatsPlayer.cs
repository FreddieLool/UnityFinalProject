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
        Luck,
        HpRegen,
        Level,
        XP,
        XpTpLevelUp;

    private Unit _playerUnit;

    private TextMeshProUGUI _stats;

    void Start()
    {
        _stats = GetComponent<TextMeshProUGUI>();
        _playerUnit = GameObject.Find("Player").GetComponent<Unit_Handeler>().Unit;
    }

    
    private void FixedUpdate()
    {
        HP = _playerUnit.HP.Value;
        DMG = _playerUnit.DMG.Value;
        AttackRate = _playerUnit.AttackRateMill.Value;
        Speed = _playerUnit.Speed.Value;
        HpRegen = _playerUnit.HpRegen.Value;
        Luck = _playerUnit.Luck.Value;
        XP = _playerUnit.XP;
        Level = _playerUnit.Level;
        XpTpLevelUp = _playerUnit.XpToLevelUp;
    }

    private void Update()
    {
        _stats.text =
            "\n"+
            $"Health : {NiceVal(HP)} \n\n"+
            $"Damage : {NiceVal(DMG)} \n\n"+
            $"AttackRate : {NiceVal(AttackRate)} \n\n"+
            $"Speed : {NiceVal(Speed)}\n\n"+
            $"HpRegen : {NiceVal(HpRegen)}\n\n"+
            $"Luck : {NiceVal(Luck)}\n\n"+
            $"Level : {NiceVal(Level)}\n\n"+
            $"XP : {NiceVal(XP)} / {NiceVal(XpTpLevelUp)}";
    }

    public static float NiceVal(float val)
    { 
        if(val % 1 == 0) { return val; }
        else { return val - (val % 0.1f); }        
    }
}
