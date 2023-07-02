using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpGUI : MonoBehaviour
{
    Unit playerUnit;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject LevelUpPanel;

    public TextMeshProUGUI[] Texts;
    public Button[] buttons;

    List<Attribute> threeAtt;

    bool playerPicked = false;

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            buttons[i].onClick.AddListener(PlayerChoosing);
        }
        playerUnit = PlayerObject.GetComponent<Unit_Handeler>().unit;
    }

    private void FixedUpdate()
    {
        if (playerUnit.LeveledUp == true)
        {
            GameOver.PauseGame();
            ApplyLevelUp();
            PlayerChoosing();

            if (playerPicked)
            {
                ApplyUpgrades();
            }
            playerUnit.LeveledUp = false;
            GameOver.ResumeGame();
        }
    }
    public void ApplyLevelUp()
    {
        List<Attribute> copyAttList = new List<Attribute>(playerUnit.AttList);
        threeAtt = new List<Attribute>();

        for (int i = 0; i < 3; i++)
        {
            int randoNum = Random.Range(0, copyAttList.Count);
            threeAtt.Add(copyAttList[randoNum]);
            Texts[i].text = $"Upgrade : {threeAtt[i].Name}";
            copyAttList.RemoveAt(randoNum);
        }

        LevelUpPanel.SetActive(true);
    }

    //public Dictionary<>


    //public int PlayerChose(int value)
    //{
    //    return value;
    //}



    void PlayerChoosing()
    {
        while (LevelUpButton.ClickedButtonName == "Nun")
        {

        }
    }

    void ApplyUpgrades()
    {
        foreach (var item in threeAtt)
        {
            for (int i = 0; i < playerUnit.AttList.Count; i++)
            {
                if (item == playerUnit.AttList[i])
                {
                    playerUnit.AttList[i] = item;
                }
            }
        }
    }
}
