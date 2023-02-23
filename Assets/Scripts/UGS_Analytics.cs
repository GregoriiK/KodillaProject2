using System.Collections;
using System.Collections.Generic;
using System;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;

public class UGS_Analytics : Singleton<UGS_Analytics>
{
    private string parametersName;
    private int parametersValue;
    async void Start()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }

        catch (ConsentCheckException e)
        {
            Debug.Log(e.ToString());
        }   
    }

    private void Update()
    {        
        parametersValue = GameplayManager.Instance.missCount;
    }

    private void OnDestroy()
    {   
        parametersName = "missCount";
        Dictionary<string, object> parameters = new Dictionary<string, object>();
        parameters.Add(parametersName, parametersValue);
        AnalyticsService.Instance.CustomData("missTarget", parameters);
        AnalyticsService.Instance.Flush();
        Debug.Log("event send");
    }

}
