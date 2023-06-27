using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;

public class ChestClosed : MonoBehaviour
{
    private Stopwatch chestDespawn = new Stopwatch();
    private float cloasedMill = 5000;

    void Start()
    {
        chestDespawn.Start();
    }

    void Update()
    {
        if(chestDespawn.ElapsedMilliseconds >= cloasedMill)
        {
            chestDespawn.Stop();
            Destroy(this.gameObject);
        }
    }
}
