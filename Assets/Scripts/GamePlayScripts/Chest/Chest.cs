using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    private GameObject _playeGO;
    [SerializeField] GameObject CloseChest;
    private Unit _playerUnit;

    private void Start()
    {
        _playeGO = GameObject.Find("Player");
        _playerUnit = _playeGO.GetComponent<Unit_Handeler>().unit;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _playeGO)
        {
            GiveReward();
            Instantiate(CloseChest, this.transform.position, this.transform.rotation);
            Destroy(this.gameObject);
        }
    }
    
    public void GiveReward()
    {
        // 1 = hp , 2 = xp , 3 = score
        
        int num = Random.Range(1, 4);
        switch(num)
        {
            case 1:
                _playerUnit.HP.Value += 25 + 7.5f * _playerUnit.Level;
                return;
            case 2:
                _playerUnit.AddXP(50 + 15 * _playerUnit.Level);
                return;
            case 3:
                ScorePlayer.PLAYER_SCORE += 100 + 75 * _playerUnit.Level;
                return;
        }
    }
}
