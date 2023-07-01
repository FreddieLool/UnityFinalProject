using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class CheatCodes : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] GameObject enemy;
    [SerializeField] TextMeshProUGUI inputField;
    ScorePlayer playerScore;
    private string input;


    // Start is called before the first frame update
    void Start()
    {
        playerScore = GetComponent<ScorePlayer>();
    }


    public void ReadStringInput()
    {
        input = inputField.text;

        Debug.Log(input);

        switch (input)
        {
            case "GoCrazy":
                player.transform.DOScaleX(10, 10).SetEase(Ease.InOutBounce).SetLoops(5, LoopType.Yoyo);
                break;
            case "PartyTime":
                enemy.transform.DORotate(new Vector2(0, 360), 10, RotateMode.FastBeyond360).SetLoops(5, LoopType.Yoyo).SetEase(Ease.Linear);
                break;
            default:
                break;
        }

    }



 
}
