using UnityEngine;
using Facebook.Unity;
using FunGames.Sdk;
using FunGamesSdk.FunGames.AppTrackingManager;
using System;

namespace FunGamesSdk.FunGames.Analytics.Helpers
{
    public class FacebookHelpers
    {
        public static void Initialize ()
        {
            var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");

            if (settings.useFacebook == false)
            {
                return;
            }
            
            if (!FB.IsInitialized) {
                FB.Init(settings.facebookGameID, null, true, true, true, false,
                    true, null, "en_US", OnHideUnity, InitCallback);
            } else {
#if UNITY_IOS && !UNITY_EDITOR
                FB.Mobile.SetAdvertiserTrackingEnabled(FunGamesAppTrackingTransparency._instance.isTrackingAllow());
                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(FunGamesAppTrackingTransparency._instance.isTrackingAllow());
#endif
                FB.ActivateApp();
            }
        }

        private static void InitCallback ()
        {
            if (FB.IsInitialized)
            {
#if UNITY_IOS && !UNITY_EDITOR
                FB.Mobile.SetAdvertiserTrackingEnabled(FunGamesAppTrackingTransparency._instance.isTrackingAllow());
                AudienceNetwork.AdSettings.SetAdvertiserTrackingEnabled(FunGamesAppTrackingTransparency._instance.isTrackingAllow());
#endif
                FB.ActivateApp();
            }
            else
            {
                Debug.Log("Failed to Initialize the Facebook SDK");
            }
        }
        
        private static void OnHideUnity (bool isGameShown)
        {
            if (!isGameShown) {
                // Pause the game - we will need to hide
            } else {
                // Resume the game - we're getting focus again
            }
        }
    }
}