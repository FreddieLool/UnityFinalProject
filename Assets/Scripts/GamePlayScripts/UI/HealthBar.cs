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
        _playerUnit = Player.GetComponent<Unit_Handeler>().unit;
    }
    private void Update()
    {
        if (_playerUnit.HP.Value > _maxHP)
        {
            SetHP();
        }
        UpdateHP();
    }
    private void SetHP()
    {
        _maxHP = _playerUnit.HP.Value;
        slider.value = _maxHP;
    }
    private void UpdateHP()
    {
        slider.value = _playerUnit.HP.Value / _maxHP;
    }


}
