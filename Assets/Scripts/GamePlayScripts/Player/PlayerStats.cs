using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] GameObject StatsMenu;

    private bool
        _activateStatsBar,
        _isKeyUp;

    // getting ( in update for max response rate ) if the button is pressed . if so ,
    // activate / deactivate the stats bar.
    void Update()
    {
        _isKeyUp = Input.GetKeyUp(KeyCode.I);

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
