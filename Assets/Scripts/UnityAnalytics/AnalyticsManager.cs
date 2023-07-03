using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AnalyticsManager : MonoBehaviour
{
    [SerializeField] GameObject PlayerObject;
    [SerializeField] GameObject GameOverUI;
    Unit_Handeler unit_Handeler;
    GameOver gameOver;


    async void Start()
    {
        unit_Handeler = PlayerObject.GetComponent<Unit_Handeler>();
        gameOver = GameOverUI.GetComponent<GameOver>();

        instance = this;
        DontDestroyOnLoad(gameObject);
        Debug.Log("First");
        await UnityServices.InitializeAsync();
        Debug.Log("Second");
        try
        {
            List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
            if (consentIdentifiers.Count > 0)
            {
                foreach (string consentIdentifier in consentIdentifiers)
                {
                    Debug.Log(consentIdentifier);
                }
            }
            else
            {
                Debug.Log("No need for any consent for analytics!");
            }
        }
        catch (ConsentCheckException exception)
        {
            Debug.LogError("Expection with checking constents! " + Environment.NewLine + exception.Message);
        }
    }

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
