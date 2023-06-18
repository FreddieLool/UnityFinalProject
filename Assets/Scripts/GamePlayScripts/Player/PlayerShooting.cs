using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class PlayerShooting : MonoBehaviour
{
    public Transform ShootingPoint;
    public GameObject bulletPrefab;
    public float BulletForce = 20;


    // attack speed stuff : ( each time shooting timer gets to the mill , player can shoot)
    // FOR NOW ATTACK SPEED IS IN HERE ( later we will prob have a status class which as will be there)
    public float AttackSpeedMill = 250;
    private Stopwatch ShootingTimer = new Stopwatch();

    public void Update()
    {
        if(ShootingTimer.ElapsedMilliseconds >= AttackSpeedMill)
        {
            ShootingTimer.Stop();
        }

        if(Input.GetButton("Fire1") && !ShootingTimer.IsRunning)
        {
            ShootingTimer.Restart();
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
