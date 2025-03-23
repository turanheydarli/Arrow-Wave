using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
using GameAnalyticsSDK;
using FunGames.Sdk.Analytics;


public class Analytics_Control : MonoBehaviour {

    public bool Gameplay_Analytics = false;

    private void Awake() {
        // GameAnalytics.OnRemoteConfigsUpdatedEvent += On_Remote_Config_Updated;
        GameAnalytics.Initialize();
    }

    // Remote Config ------------------------------------------------------

    // Set_Interstitial_Interval
    private static void On_Remote_Config_Updated() {
        // string interstitial_interval = GameAnalytics.GetRemoteConfigsValueAsString ("Intstl_Int", "30");
        // GDG.Monetization_Control.Set_Interstitial_Interval(interstitial_interval);
    }

    // Util ---------------------------------------------------------------

    private int Level_Index {
        get {
            return GDG.Game_Control.Current_Level_Index;
        }
    }

    private string Level_Index_Str {
        get {
            return "Level" + Level_Index.ToString("D3");
        }
    }

    private string Level_Str {
        get {
            return Level_Index_Str + "_" + Level_Scene_Name;
        }
    }

    private string Level_Scene_Name {
        get {
            return GDG.Game_Control.Current_Scene_Name;
        }
    }

    private string Level_Section {
        get {
            return "default_section";
        }
    }


    // Game Events --------------------------------------------------------

    public void Send_Level_Start() {
        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Level_Str);
        FunGamesAnalytics.NewProgressionEvent("Start", Level_Index_Str);
    }

    public void Send_Level_Complete() {
        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Level_Str);
        FunGamesAnalytics.NewProgressionEvent("Complete", Level_Index_Str);
    }

    public void Send_Level_Fail() {
        FunGamesAnalytics.NewProgressionEvent("Fail", Level_Index_Str);
        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Fail, Level_Str);
    }

    public void Send_Level_Skip() {
        // GameAnalytics.NewDesignEvent("Level_Skip:" + Level_Str);
    }

    public void Send_Level_Section_Start() {
        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Start, Level_Str, Level_Section);
    }

    public void Send_Level_Section_Complete() {
        // GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, Level_Str, Level_Section);
    }

    public void Send_Watch_Ad_Clicked_On_Unlock_Screen(string item_name) {
        GameAnalytics.NewDesignEvent("Watch_Ad_On_Unlock:" + item_name);
    }

    public void Send_Buy_Now_Clicked_On_Unlock_Screen(string item_name) {
        GameAnalytics.NewDesignEvent("Buy_Now_On_Unlock:" + item_name);
    }

    public void Send_Skip_Clicked_On_Unlock_Screen(string item_name) {
        GameAnalytics.NewDesignEvent("Skip_On_Unlock:" + item_name);
    }

    public void Send_Watch_Ad_Clicked_On_Shop(string item_name) {
        GameAnalytics.NewDesignEvent("Watch_Ad_On_Shop:" + item_name);
    }

    public void Send_Buy_Now_Clicked_On_Shop(string item_name) {
        GameAnalytics.NewDesignEvent("Buy_Now_On_Shop:" + item_name);
    }

    public void Send_Interstitial_Ad_Show() {
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial, "AdMob", "General");
    }

    public void Send_Rewarded_Ad_Show() {
        GameAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo, "AdMob", "General");
    }

    public void Send_Rewarded_Ad_Watched() {
        GameAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo, "AdMob", "General");
    }



}
