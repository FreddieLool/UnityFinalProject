using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    Unit_Handeler unit_Handeler;
    GameOver gameOver;


    public static AnalyticsManager Instance
    {
        get
        {
            return instance;
        }
        private set
        {
            instance = value;
        }
    }

    private static AnalyticsManager instance;


    public void Update()
    {
        ReportAmountsRevived();
        ReportEnemiesKilled();
        
    }

    public void ReportEnemiesKilled()
    {
        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("enemiesKilled", unit_Handeler.EnemiesKilled);

        AnalyticsService.Instance.CustomData("howManyKilled", eventParameters);
    }

    public void ReportAmountsRevived()
    {
        Dictionary<string, object> eventParameters = new Dictionary<string, object>();
        eventParameters.Add("AmountsRevived", gameOver.AmountsRevived);

        AnalyticsService.Instance.CustomData("AmountsRevived", eventParameters);
    }


}
