using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameObject StatsMenu;

    private bool
        _activateStatsBar;


    // getting ( in update for max response rate ) if the button is pressed . if so ,
    // activate / deactivate the stats bar.
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            _activateStatsBar = !_activateStatsBar;
        }
    }

    // if its time to enable / disable ( in fixed update ) do it :
    private void FixedUpdate()
    {
        if(StatsMenu.activeSelf != _activateStatsBar)
        {
            StatsMenu.SetActive(_activateStatsBar);
        }
    }
}
