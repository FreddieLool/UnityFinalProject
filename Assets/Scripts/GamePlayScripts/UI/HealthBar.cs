using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    [SerializeField] GameObject Player;
    private Unit _playerUnit = null;
    private float _maxHP;
    private void Update()
    {
        if (Player.GetComponent<Unit_Handeler>() == null)
        {
            return;
        }
        else
        {
            if (_playerUnit == null)
            {
                _playerUnit = Player.GetComponent<Unit_Handeler>().unit;
                SetHP();
                _maxHP = _playerUnit.HP.Value;
            }
            UpdateHP();
        }

    }
    public void SetHP()
    {
        slider.value = _playerUnit.HP.Value ;
    }
    public void UpdateHP()
    {
        slider.value = _playerUnit.HP.Value / _maxHP;
    }


}
