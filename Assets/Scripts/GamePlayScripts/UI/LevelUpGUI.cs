using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpGUI : MonoBehaviour
{
    Unit playerUnit;
    [SerializeField] GameObject LevelUpPanel;

    public TextMeshProUGUI[] Texts;
    public Button[] buttons;

    List<Attribute> threeAtt;

    private void FixedUpdate()
    {
        if (playerUnit.LeveledUp == true)
        {
            ApplyLevelUp();
        }
    }
    public void ApplyLevelUp()
    {
        List<Attribute> copyAttList = new List<Attribute>();
        List<Attribute> threeAtt = new List<Attribute>();

        playerUnit.AttList.CopyTo(copyAttList.ToArray(), 0);

        for (int i = 0; i < 3; i++)
        {
           threeAtt[i] = copyAttList[Random.Range(0, copyAttList.Count)];
            Texts[i].text = threeAtt.ToString();
            copyAttList.RemoveAt(i);
        }

        LevelUpPanel.SetActive(true);
    }


    public void TaskOnClick()
    {
        
    }
    
}
