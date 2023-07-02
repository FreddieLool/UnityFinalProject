using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] GameObject Player;
    private Unit _playerUnit;
    private float _maxHP = 0;
    private void Start()
    {
        _playerUnit = Player.GetComponent<Unit_Handeler>().Unit;
    }
    private void FixedUpdate()
    {
        if (_playerUnit.HP.Value > _maxHP)
        {
            SetMaxHP();
        }
        UpdateHP();
    }
    private void SetMaxHP()
    {
        _maxHP = _playerUnit.HP.Value;
        slider.value = _maxHP;
    }
    private void UpdateHP()
    {
        slider.value = (_playerUnit.HP.Value / _maxHP );
        float f2 = 0;
        for (float f = 0.90f;  f > slider.value; f-= 0.10f , f2+= 0.0215f) { }
        slider.value += f2;
    }


}
