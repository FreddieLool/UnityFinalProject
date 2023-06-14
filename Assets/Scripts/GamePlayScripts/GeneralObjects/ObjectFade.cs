using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
public class ObjectFade : MonoBehaviour
{
    private Stopwatch _fadeTimer = new Stopwatch();
    private float _fadeMill = 750;
    private Color _color;
    // faded and unfaded alphas :
    private float faded = 0.45f, unfaded = 1;

    private void Start()
    {
        _color = this.gameObject.GetComponent<SpriteRenderer>().color;
    }
    

    private void OnTriggerStay2D(Collider2D collision)
    {
        //applying faded color:
        if (_color.a != faded)
        {
            _color.a = faded;
            this.gameObject.GetComponent<SpriteRenderer>().color = _color;
        }
        //while something is inside the objext sprite , keep it faded :
        _fadeTimer.Reset();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        _fadeTimer.Start();
    }

    private void FixedUpdate()
    {
        if(_fadeTimer.ElapsedMilliseconds >= _fadeMill)
        {
            _fadeTimer.Reset();
            // applying back the original color:
            _color.a = unfaded;
            this.gameObject.GetComponent<SpriteRenderer>().color = _color;
        }
    }
}
