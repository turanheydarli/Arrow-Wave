using System;
using UnityEngine;
using System.Collections.Generic;
using GameAnalyticsSDK.Utilities;
using com.adjust.sdk;

public class FungamesAdjustMaxEvent : MonoBehaviour
{
    public string AdjustEventInterID;
    public string AdjustEventRewardedID;

    [Header("Retention Event")]
    public string AdjustTokenRet1;
    public string AdjustTokenRet3;
    public string AdjustTokenRet5;
    public string AdjustTokenRet7;

    [Header("Rewarded event")]
    public int nbRV1 = 5;
    public string nbRV1Token;

    [Space]
    public int nbRV2 = 10;
    public string nbRV2Token;

    [Space]
    public int nbRV3 = 15;
    public string nbRV3Token;

    [Space]
    public int nbRV4 = 20;
    public string nbRV4Token;

    [Space]
    public int nbRV5 = 25;
    public string nbRV5Token;

    private int daysSinceFirstConnection;
    private int daysSinceLastConnection;
    private int nbSessionToday;
    private int nbRV;

    
#if !(UNITY_EDITOR)
    public void Awake()
    {
        // Retention
        if (PlayerPrefs.HasKey("dateFirstCo"))
        {
            DateTime store = Convert.ToDateTime(PlayerPrefs.GetString("dateFirstCo"));
            DateTime today = DateTime.Now;

            TimeSpan elapsed = today.Subtract(store);
            daysSinceFirstConnection = (int)elapsed.TotalDays;
            if (daysSinceFirstConnection == 1)
            {
                if (!PlayerPrefs.HasKey("AdjustTokenRet1"))
                {
                    PlayerPrefs.SetInt("AdjustTokenRet1", 1);
                    AdjustEvent adjustEvent = new AdjustEvent(AdjustTokenRet1);
                    Adjust.trackEvent(adjustEvent);
                }
            }
            else if (daysSinceFirstConnection == 3)
            {
                if (!PlayerPrefs.HasKey("AdjustTokenRet3"))
                {
                    PlayerPrefs.SetInt("AdjustTokenRet3", 3);
                    AdjustEvent adjustEvent = new AdjustEvent(AdjustTokenRet3);
                    Adjust.trackEvent(adjustEvent);
                }
            }
            else if (daysSinceFirstConnection == 5)
            {
                if (!PlayerPrefs.HasKey("AdjustTokenRet5"))
                {
                    PlayerPrefs.SetInt("AdjustTokenRet5", 5);
                    AdjustEvent adjustEvent = new AdjustEvent(AdjustTokenRet5);
                    Adjust.trackEvent(adjustEvent);
                }
            }
            else if (daysSinceFirstConnection == 7)
            {
                if (!PlayerPrefs.HasKey("AdjustTokenRet7"))
                {
                    PlayerPrefs.SetInt("AdjustTokenRet7", 7);
                    AdjustEvent adjustEvent = new AdjustEvent(AdjustTokenRet7);
                    Adjust.trackEvent(adjustEvent);
                }
            }
        }
        else
        {

            PlayerPrefs.SetInt("playtime", 0);
            PlayerPrefs.SetString("dateFirstCo", DateTime.Now.ToString());
            daysSinceFirstConnection = 0;
        }

        // NB Session
        if (PlayerPrefs.HasKey("dateLastCo"))
        {
            DateTime store = Convert.ToDateTime(PlayerPrefs.GetString("dateLastCo"));
            PlayerPrefs.SetString("dateLastCo", DateTime.Now.ToString());
            DateTime today = DateTime.Now;

            TimeSpan elapsed = today.Subtract(store);
            daysSinceLastConnection = (int)elapsed.TotalDays;
        }
        else
        {
            PlayerPrefs.SetString("dateLastCo", DateTime.Now.ToString());
            daysSinceLastConnection = 0;
        }

        if (PlayerPrefs.HasKey("nbSession"))
        {
            if (daysSinceLastConnection != 0)
            {
                PlayerPrefs.SetInt("nbSession", 0);
                nbSessionToday = 0;
            }
            else
            {
                nbSessionToday = PlayerPrefs.GetInt("nbSession");
                nbSessionToday = nbSessionToday + 1;
                PlayerPrefs.SetInt("nbSession", nbSessionToday);

            }
        }
        else
        {
            PlayerPrefs.SetInt("nbSession", 0);
            nbSessionToday = 0;
        }


        /*if (PlayerPrefs.HasKey("retention"))
        {
            PlayerPrefs.SetInt("retention", PlayerPrefs.GetInt("retention")+1);
        }
        else
        {
            PlayerPrefs.SetInt("retention", 0);
            nbSessionToday = 0;
        }*/

        if (PlayerPrefs.HasKey("nbRV"))
        {
            PlayerPrefs.SetInt("nbRV", 0);
        }
        
        PlayerPrefs.SetInt("sessionLenght", 0);


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


    private void runCallback(string format, MaxSdkBase.AdInfo adInfo)
    {
        if(format == "INTER")
        {
            AdjustEvent adjustEvent = new AdjustEvent(AdjustEventInterID);
            adjustEvent.setRevenue(0, "USD");
            Adjust.trackEvent(adjustEvent);
        }
        else if(format == "REWARDED")
        {
            nbRV = PlayerPrefs.GetInt("nbRV") + 1;
            PlayerPrefs.SetInt("nbRV", nbRV);
            AdjustEvent adjustEvent = new AdjustEvent(AdjustEventRewardedID);
            adjustEvent.setRevenue(0, "USD");
            Adjust.trackEvent(adjustEvent);
            if (nbRV == nbRV1)
            {
                Adjust.trackEvent(new AdjustEvent(nbRV1Token));
            }
            else if (nbRV == nbRV2)
            {
                Adjust.trackEvent(new AdjustEvent(nbRV2Token));
            }

            else if (nbRV == nbRV3)
            {
                Adjust.trackEvent(new AdjustEvent(nbRV3Token));
            }

            else if (nbRV == nbRV4)
            {
                Adjust.trackEvent(new AdjustEvent(nbRV4Token));
            }

            else if (nbRV == nbRV5)
            {
                Adjust.trackEvent(new AdjustEvent(nbRV5Token));
            }
        }

        // initialise with AppLovin MAX source
        AdjustAdRevenue adjustAdRevenue = new AdjustAdRevenue(AdjustConfig.AdjustAdRevenueSourceAppLovinMAX);
        // set revenue and currency
        adjustAdRevenue.setRevenue(adInfo.Revenue, "USD");
        // optional parameters
        adjustAdRevenue.setAdRevenueNetwork(adInfo.NetworkName);
        adjustAdRevenue.setAdRevenueUnit(adInfo.AdUnitIdentifier);
        adjustAdRevenue.setAdRevenuePlacement(adInfo.Placement);
        // track ad revenue
        Adjust.trackAdRevenue(adjustAdRevenue);

    }
#endif
}
