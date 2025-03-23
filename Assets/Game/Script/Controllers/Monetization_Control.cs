using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FunGames.Sdk.Ads;

public class Monetization_Control : MonoBehaviour {

    public float Ad_Watch_Interval = 30f;
    private float Last_Ad_Watch_Time;

    private Action RV_Success_Callback;
    // private Action RV_Fail_Callback;

    public bool Show_Ads = true;


    void Start() {
        Show_Banner();
    }

    private void Show_Banner() {
        if(Show_Ads) {
            FunGamesMax.ShowBannerAd();
        }
    }

    public void Show_Interstitial() {
        if(Time.unscaledTime - Last_Ad_Watch_Time < Ad_Watch_Interval) { return; }
        if(Show_Ads) {
            FunGamesMax.ShowInterstitial(Interstitial_Action);
        } else {
            Interstitial_Action("", "", 0);
        }
        Debug.Log("Interstitial triggered!");
    }

    private void Interstitial_Action(string status, string argString, int argInt) {
        Last_Ad_Watch_Time = Time.unscaledTime;
    }

    public void Show_Rewarded_Video(Action success_callback) {
        RV_Success_Callback = success_callback;
        if(Show_Ads) {
            FunGamesMax.ShowRewarded(Rewarded_Action);
        } else {
            RV_Success_Callback?.Invoke();
        }
        Debug.Log("RV triggered!");
    }

    private void Rewarded_Action(string status, string strArgs, int earned) {
        if(status == "success") {
            Last_Ad_Watch_Time = Time.unscaledTime;
            RV_Success_Callback?.Invoke();
            Debug.Log("RV watched!");
        }
    }

}
