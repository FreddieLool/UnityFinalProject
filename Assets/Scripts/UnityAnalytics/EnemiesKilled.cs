using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemiesKilled : MonoBehaviour
{
    // Send custom event
    Dictionary<string, object> parameters = new Dictionary<string, object>()
{
    { "fabulousString", "hello there" },
    { "sparklingInt", 1337 },
    { "spectacularFloat", 0.451f },
    { "peculiarBool", true },
};
    // The ‘myEvent’ event will get queued up and sent every minute
    //AnalyticsService.Instance.CustomData("myEvent", parameters); 
}
