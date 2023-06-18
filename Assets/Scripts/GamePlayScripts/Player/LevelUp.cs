using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUp : MonoBehaviour
{
    [SerializeField] private GameObject lvlUpPanel;
    public int level = 0;
    public float currentXP = 0;
    public float maxXP = 30;

    void Update()
    {
        LvlUp();
    }
    void LvlUp()
    {
        if (currentXP >= maxXP)
        {
            currentXP = currentXP - maxXP;
            level++;
            maxXP = maxXP * 1.5f;
            lvlUpPanel.SetActive(true);
            Time.timeScale = 0;
        }
    }
}
