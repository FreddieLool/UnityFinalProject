using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelUpGUI : MonoBehaviour
{
    private Unit _playerUnit;
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject LevelUpPanel;
    [SerializeField] GameObject PlayerUI;

    public TextMeshProUGUI[] Texts;
    public Button[] buttons;

    List<Attribute> threeAtt;

    private void Start()
    {
        _playerUnit = PlayerObject.GetComponent<Unit_Handeler>().Unit;
    }

    public void ActivateLevelUp()
    {
        ApplyLevelUp();
    }

    private void FixedUpdate()
    {
        PlayerChoosing();
    }

    public void ApplyLevelUp()
    {
        GameOver.PauseGame();
        _playerUnit.LeveledUp = false;
        List<Attribute> copyAttList = new List<Attribute>(_playerUnit.AttList);
        threeAtt = new List<Attribute>();

        for (int i = 0; i < 3; i++)
        {
            int randoNum = Random.Range(0, copyAttList.Count);
            threeAtt.Add(copyAttList[randoNum]);
            Texts[i].text = $"Upgrade : {threeAtt[i].Name}";
            copyAttList.RemoveAt(randoNum);
        }

        PlayerUI.SetActive(false);
        LevelUpPanel.SetActive(true);
    }

    void PlayerChoosing()
    {
        if(LevelUpButton.ClickedButtonName != "Nun") 
        {
            threeAtt[int.Parse(LevelUpButton.ClickedButtonName)].AttLvlUpUpgrade();
            ApplyUpgrades();
            LevelUpButton.ClickedButtonName = "Nun";
            LevelUpPanel.SetActive(false);
            PlayerUI.SetActive(true);
            GameOver.ResumeGame();
        }
    }

    void ApplyUpgrades()
    {
        foreach (var item in threeAtt)
        {
            for (int i = 0; i < _playerUnit.AttList.Count; i++)
            {
                if (item == _playerUnit.AttList[i])
                {
                    _playerUnit.AttList[i] = item;
                }
            }
        }
    }
}
