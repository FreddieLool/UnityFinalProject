using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] GameObject StatsMenu;
    [SerializeField] GameObject GameOverMenuUI;
    [SerializeField] GameObject Player;
    [SerializeField] GameObject Pause;
    [SerializeField] GameObject HpBar;
    [SerializeField] GameObject Score;

    private bool
        _activateStatsBar;

    private Unit _playerUnit;

    private void Awake()
    {
        Pause.SetActive(true);
        HpBar.SetActive(true);
        Score.SetActive(true);
    }

    private void Start()
    {
        _playerUnit = Player.GetComponent<Unit_Handeler>().unit;
    }

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
        if(_playerUnit.IsDead() && !GameOver.IsGamePaused)
        {
            StatsMenu.SetActive(false);
            GameOverMenuUI.SetActive(true);
            GameOverMenuUI.GetComponent<GameOver>().ApplyGameOver();
        }
    }
}
