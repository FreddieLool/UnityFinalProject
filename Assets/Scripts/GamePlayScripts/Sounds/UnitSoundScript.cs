using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitSoundScript : MonoBehaviour
{
    [SerializeField] EnemyAudioEffects UnitAudio;

    private GameObject _unitGO;

    private void Start()
    {
        UnitAudio.ZombieHit();
    }

    private void FixedUpdate()
    {
        if (!UnitAudio.src.isPlaying)
        {
            Destroy(gameObject);
        }
        else if(_unitGO != null)
        {
            this.transform.position = _unitGO.transform.position;
        }
    }

    public void SetUnit(GameObject unitGO)
    {
        _unitGO = unitGO;
    }
}
