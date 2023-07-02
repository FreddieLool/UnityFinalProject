using TMPro;
using UnityEngine;


public class Chest : MonoBehaviour
{

    [SerializeField] GameObject CloseChest;
    [SerializeField] GameObject ChestTextGO;
    private GameObject _playerGO;
    private Unit _playerUnit;
    private GameObject _chestTextCanvasGO;
    private TextMeshPro _text;
    private string _textChestStr;

    private void Start()
    {
        _playerGO = GameObject.FindGameObjectWithTag("Player");
        _chestTextCanvasGO = GameObject.Find("GameTextCanvas");
        _playerUnit = _playerGO.GetComponent<Unit_Handeler>().Unit;
        _text = _chestTextCanvasGO.GetComponent<TextMeshPro>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == _playerGO)
        {
            GiveReward();
            Instantiate(CloseChest, this.transform.position, this.transform.rotation);
            Instantiate(ChestTextGO, this.transform.position, this.transform.rotation, _chestTextCanvasGO.transform)
                .GetComponent<TextMeshPro>().text = _textChestStr;
            Destroy(this.gameObject);
        }
    }

    public void GiveReward()
    {
        // 1 = hp , 2 = xp , 3 = score

        int num = Random.Range(1, 4);
        switch (num)
        {
            case 1:
                _textChestStr = $" + {25 + 7.5f * _playerUnit.Level} HP";
                _playerUnit.HP.Value += 25 + 7.5f * _playerUnit.Level;
                return;
            case 2:
                _textChestStr = $" + {50 + 15 * _playerUnit.Level} XP";
                _playerUnit.XP += (50 + 15 * _playerUnit.Level);
                //_playerUnit.AddXP(50 + 15 * _playerUnit.Level);
                return;
            case 3:
                _textChestStr = $" + {100 + 75 * _playerUnit.Level} SCORE";
                ScorePlayer.PLAYER_SCORE += 100 + 75 * _playerUnit.Level;
                return;
        }
    }
}
