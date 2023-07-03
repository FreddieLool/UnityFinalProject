using DG.Tweening;
using TMPro;
using UnityEngine;

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
                Debug.Log(input);
                player.transform.DOScaleX(10, 10).SetEase(Ease.InOutBounce).SetLoops(-1, LoopType.Yoyo);
         
                break;
            case "PartyTime":
                Debug.Log(input);

                enemy.transform.DORotate(new Vector2(0, 360), 10, RotateMode.FastBeyond360).SetLoops(5, LoopType.Yoyo).SetEase(Ease.Linear);
           
                break;
            default:
                break;
        }

    }




}
