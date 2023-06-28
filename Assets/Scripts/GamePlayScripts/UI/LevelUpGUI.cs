using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LevelUpGUI : MonoBehaviour
{
    Unit playerUnit;

    [SerializeField] Text button1Text, button2Text, button3Text;

    public void ApplyLevelUp()
    {
        List<Attribute> copyAttList = new List<Attribute>();

        playerUnit.AttList.CopyTo(copyAttList.ToArray(), 0);

        for (int i = 0; i < 3; i++)
        {
            
        }
    }

}
