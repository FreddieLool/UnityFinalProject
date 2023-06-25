using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnitMapHpBar : MonoBehaviour
{
    private GameObject _perentGameObject;
    private Vector3 _up = new Vector3(0, 1.15f, 0);
    private Image _hpBar;
    private float _maxHp;
    private Unit _perentUnit;
   
    private void FixedUpdate()
    {
        if (_perentGameObject != null)
        {
            this.transform.SetPositionAndRotation(_perentGameObject.transform.position + _up, Quaternion.identity);

            if(_maxHp < _perentUnit.HP.Value)
            {
                _maxHp = _perentUnit.HP.Value;
            }
            if(_maxHp != _perentUnit.HP.Value)
            {
                _hpBar.fillAmount = _perentUnit.HP.Value / _maxHp;
            }
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void ActivateHpBar(GameObject perentGameObject , Unit perentUnit )
    {
        _perentUnit = perentUnit;
        _maxHp = _perentUnit.HP.Value;
        _perentGameObject = perentGameObject;
        _hpBar = transform.GetChild(0).gameObject.GetComponent<Image>();
        _hpBar.fillAmount = 1;
    }
}
