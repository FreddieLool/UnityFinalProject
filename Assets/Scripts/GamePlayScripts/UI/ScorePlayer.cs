using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Diagnostics;
using System.Linq;
using System;

public class ScorePlayer : MonoBehaviour
{
    public static float PLAYER_SCORE = 0;

    private static Dictionary<SCORE_TYPE, float> _scoreApplier = new Dictionary<SCORE_TYPE, float>
    {
        {
            SCORE_TYPE.KILL , 15
        },
        {
            SCORE_TYPE.PASSIVE , 3
        },
    };

    private static TextMeshProUGUI _scoreText;
    private static Stopwatch _updateScoreTimer = new Stopwatch();
    private static float _updateScoreMill = 1000;
    private static float _scoreModifier = 1;
    private static float _scorModAdd = 0.025f;

    private void Awake()
    {
        PLAYER_SCORE = 0;
    }

    private void Start()
    {
        _scoreText = GetComponent<TextMeshProUGUI>();
        _updateScoreTimer.Start();
    }

    // Update is called once per frame
    void Update()
    {
        _scoreText.text = $"Score   {(int)PLAYER_SCORE}";
    }

    private void FixedUpdate()
    {
        if(_updateScoreTimer.ElapsedMilliseconds >= _updateScoreMill)
        {
            _updateScoreTimer.Restart();
            _scoreModifier += _scorModAdd;
            AddScore(SCORE_TYPE.PASSIVE);
        }
    }

    public static void AddScore(SCORE_TYPE st)
    {
        PLAYER_SCORE += (_scoreApplier[st] * _scoreModifier);
    }

}


public enum SCORE_TYPE
{
    KILL,
    PASSIVE,
    // ADD MORE LATER.
}


