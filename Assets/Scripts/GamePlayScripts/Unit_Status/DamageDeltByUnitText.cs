using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Diagnostics;
using TMPro;

public class DamageDeltByUnitText : MonoBehaviour
{
    private float _aliveMill;
    private TextMeshPro _text;
    private GameObject _perentUnit;
    private Vector3 _up = new Vector3(0, 1.80f , 0);
    private float _alpha = 1;
    private float _animTimeDelay = 0.015f;
    private float _reduceInCorotine;


    private void Start()
    {
        StartCoroutine(thisAnimation());
    }

    private void FixedUpdate()
    {
        if (_perentUnit != null)
        {
            this.transform.SetPositionAndRotation(_perentUnit.transform.position + _up, Quaternion.identity);
        }
    }


    private IEnumerator thisAnimation()
    {
        for (float f = _aliveMill; f > 0; f -= _animTimeDelay)
        {
            if (_perentUnit == null)
            {
                Destroy(this.gameObject);
                yield break;
            }

            this.transform.localScale -= new Vector3(_reduceInCorotine, _reduceInCorotine, 0);
            _alpha -= (_reduceInCorotine);
            _text.color = new Color(_text.color.r, _text.color.g , _text.color.b  , _alpha);
            yield return new WaitForSecondsRealtime(_animTimeDelay);
        }
        Destroy(this.gameObject);
        yield break;
    }


    public void ActivateDamageText(float damage , GameObject perentUnit , float aliveMill)
    {
        _aliveMill = aliveMill/1000;
        _perentUnit = perentUnit;
        _text = GetComponent<TextMeshPro>();
        _text.text = damage.ToString();
        _reduceInCorotine = 1 / (_aliveMill/ _animTimeDelay);
    }

}
