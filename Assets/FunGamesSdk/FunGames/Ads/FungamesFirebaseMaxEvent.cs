using System;
using UnityEngine;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;
using com.adjust.sdk;
using Firebase.Analytics;

public class FungamesFirebaseMaxEvent : MonoBehaviour
{
#if !(UNITY_EDITOR)
    public void Awake()
    {
        if (_subscribed)
        {
            Debug.Log("Ignoring duplicate adjust max subscription");
            return;
        }

        MaxSdkCallbacks.Interstitial.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("INTER", adInfo);
        MaxSdkCallbacks.Banner.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("BANNER", adInfo);
        MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("REWARDED", adInfo);
        MaxSdkCallbacks.CrossPromo.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("XPROMO", adInfo);
        MaxSdkCallbacks.MRec.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("MREC", adInfo);
        MaxSdkCallbacks.RewardedInterstitial.OnAdRevenuePaidEvent += (adUnitId, adInfo) => runCallback("REWARDED_INTER", adInfo);
        _subscribed = true;
    }

    
    private static bool _subscribed = false;


    private static void runCallback(string format, MaxSdkBase.AdInfo adInfo)
    {
        Firebase.Analytics.Parameter[] AdParameters = {
          new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdPlatform, "AppLovin"),
          new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdSource, adInfo.NetworkName),
          new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdUnitName, adInfo.AdUnitIdentifier),
          new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterAdFormat, format),
          new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterCurrency,"USD"),
          new Firebase.Analytics.Parameter(FirebaseAnalytics.ParameterValue, adInfo.Revenue)
        };
        Firebase.Analytics.FirebaseAnalytics.LogEvent("custom_ad_impression", AdParameters);
        Firebase.Analytics.FirebaseAnalytics.LogEvent(FirebaseAnalytics.EventAdImpression, AdParameters);

    }
#endif
}