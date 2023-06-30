using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;


public class PlayerShooting : MonoBehaviour
{
    [SerializeField] UNIT_TAG UnitTag;
    private Unit _playerUnit;
    public Transform ShootingPoint;
    public GameObject bulletPrefab;
    public float BulletForce = 55;


    // attack speed stuff : ( each time shooting timer gets to the mill , player can shoot)
   
    public void Start()
    {
        _playerUnit = GetComponent<Unit_Handeler>().unit;
    }

    public void Update()
    {
        if (GameOver.IsGamePaused) { return; }

        if (_playerUnit.AttackRateTimer.ElapsedMilliseconds >= _playerUnit.AttackRateMill.Value)
        {
            _playerUnit.AttackRateTimer.Stop();
        }

        if(Input.GetButton("Fire1") && !_playerUnit.AttackRateTimer.IsRunning)
        {
            _playerUnit.AttackRateTimer.Restart();
            Shoot();
        }
    }

    public void Shoot()
    {
        GameObject newBullet = 
        Instantiate(bulletPrefab, ShootingPoint.position, ShootingPoint.rotation);
        newBullet.GetComponent<Rigidbody2D>().AddForce(ShootingPoint.up * BulletForce, ForceMode2D.Impulse);

        newBullet.AddComponent<Bullet>().Owner = this.gameObject;
    }
}
