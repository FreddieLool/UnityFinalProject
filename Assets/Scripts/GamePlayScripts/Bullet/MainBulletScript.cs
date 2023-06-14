using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class MainBulletScript : MonoBehaviour
{
    private Stopwatch _bulletAliveTime = new Stopwatch();
    private float _timeAliveMill = 2000;
  

    // handeling collision:
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // for now!!
        if (!collision.gameObject.name.Contains("Bullet"))
        {
            Destroy(gameObject);
        }
    }

    // handeling despawn timer :
    private void FixedUpdate()
    {
        if(!_bulletAliveTime.IsRunning)
        {
            _bulletAliveTime.Start();
        }
        if(_bulletAliveTime.ElapsedMilliseconds >= _timeAliveMill)
        {
            Destroy(gameObject);
        }
    }

    //// the timer that counts the mill :
    //private Stopwatch _bulletAliveTime;
    //// how much time ( in millisec ) the bullet have before he gets deleted.
    //public float TimeAliveMill = 2000;

    //void Start()
    //{
    //    _bulletAliveTime.Start();
    //}

    //// Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (_bulletAliveTime.ElapsedMilliseconds >= TimeAliveMill)
    //    {
    //        Destroy(gameObject);
    //    }
    //}
}
