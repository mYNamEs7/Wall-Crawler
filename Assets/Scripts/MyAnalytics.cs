using System;
using System.Collections;
using System.Collections.Generic;
using GameAnalyticsSDK;
using GameAnalyticsSDK.Events;
using GameAnalyticsSDK.State;
using UnityEngine;
using YG;

public class MyAnalytics : MonoBehaviour
{
    private void OnEnable()
    {
        // GameAnalytics.OnRemoteConfigsUpdatedEvent += Bot;
        
        GameAnalytics.Initialize();
        
        GAState.Init();
    
        GameAnalytics.RemoteConfigsUpdated();
        
        // Bot();
    }

    public static void OnLevelStart(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, $"{level}");
        Debug.Log($"GA Level Start {level}");
    }

    public static void OnLevelComplete(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, $"{level}");
        Debug.Log($"GA Level Complete {level}");
    }
    
    public static void OnLevelRestart(int level)
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, $"{level}");
        Debug.Log($"GA Level Fail {level}");
    }

    // private void Bot()
    // {
    //     if (int.TryParse(YandexGame.GetFlag("BotAI"), out var a))
    //     {
    //         BotAI = a;
    //         Debug.Log($"BotAI = {BotAI}");
    //     }
    //     else
    //         Debug.Log($"BotAI = null ({YandexGame.GetFlag("BotAI")})");
    //
    //     // StartCoroutine(Printing());
    // }
}
