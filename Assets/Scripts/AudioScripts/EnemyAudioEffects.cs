using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAudioEffects : MonoBehaviour
{
    [SerializeField] AudioClip zombieHitSFX, zombieDeathSFX;

    public AudioSource src;

    public void ZombieHit()
    {
        src.clip = zombieHitSFX;
        src.Play();
    }

    public void ZombieDeath()
    {
        src.clip = zombieDeathSFX;
        src.Play();
    }
}
