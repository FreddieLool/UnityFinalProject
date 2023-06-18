using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class Bullet : MonoBehaviour
{
    public GameObject Owner { get; set; }
    public GameObject ownerShow;
    public void FixedUpdate()
    {
        ownerShow = Owner;
    }
}
