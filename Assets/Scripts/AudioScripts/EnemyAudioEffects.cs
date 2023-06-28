using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioEffects : MonoBehaviour
{
    [SerializeField] AudioClip zombieSFX1, zombieSFX2, zombieSFX3;
    [SerializeField] AudioSource src;

    public void ZombieHit()
    {
        src.clip = zombieSFX1;
        src.Play();
    }


    public void ZombieAttack()
    {
        src.clip = zombieSFX2;
        src.Play();
    }

    public void ZombieRandomScream()
    {
        src.clip = zombieSFX3;
        src.Play();
    }
}
