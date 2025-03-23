using Unity.Collections;
using UnityEngine;

namespace FunGamesSdk
{
    [CreateAssetMenu(fileName = "Assets/Resources/FunGamesSettings", menuName = "FunGamesSdk/Settings", order = 1000)]
    public class FunGamesSettings : ScriptableObject
    {
        [Header("Sdk Version")]
        
        [Tooltip("Sdk Version")]
        [ReadOnly] public string version = "2.7";


        [Header("AppTrackingTransparency (iOS 14)")]
        
        [Tooltip("UseATTPopup")]
        public bool useATTPopup;
        
        
        [Header("PushNotifications")]
        
        [Tooltip("Use PushNotifications")]
        public bool usePushNotifications;
        
        
        [Header("Facebook")]
        
        [Tooltip("Use Facebook")]
        public bool useFacebook;
        
        [Tooltip("Facebook Game ID")]
        public string facebookGameID;

        
        [Header("GameAnalytics")]
        
        [Tooltip("Use GameAnalytics")]
        public bool useGameAnalytics;
        
        [Tooltip("GameAnalytics Ios Game Key")]
        public string gameAnalyticsIosGameKey;

        [Tooltip("GameAnalytics Ios Secret Key")]
        public string gameAnalyticsIosSecretKey;

        [Tooltip("GameAnalytics Android Game Key")]
        public string gameAnalyticsAndroidGameKey;

        [Tooltip("GameAnalytics Android Secret Key")]
        public string gameAnalyticsAndroidSecretKey;

        
        [Header("Tenjin")]
        
        [Tooltip("Use Tenjin")]
        public bool useTenjin;

        [Tooltip("Tenjin Api Key")]
        public string tenjinApiKey = "TY9WX3K6HCNJPIXYZB6ZN2RPPZEAM7QJ";


        [Header("CrossPromo")]
        
        [Tooltip("Play Local Videos")]
        public bool playLocalVideos;
        
        [Tooltip("Play Remote Videos")]
        public bool playRemoteVideos;


        [Header("Ogury")]
        [Tooltip("Use Ogury")]
        public bool useOgury;

        
        [Header("Ogury iOS")]
        
        [Tooltip("ogury iOS AssetKey")]
        public string oguryIOSAssetKey;
        
        [Tooltip("iOS thumbnail AdUnitId")]
        public string iOSThumbnailAdUnitId;


        [Header("Ogury Android")]
        
        [Tooltip("ogury Android AssetKey")]
        public string oguryAndroidAssetKey;
        
        [Tooltip("Android thumbnail AdUnitId")]
        public string androidThumbnailAdUnitId;

        [Header("GDPR")]
        [Tooltip("Use Ogury GDPR")]
        public bool useOguryGdpr;
        [Tooltip("Use TapNation GDPR")]
        public bool useTapNationGdpr;
        
        [Header("Applovin Max")]
        
        [Tooltip("Use Max")]
        public bool useMax;
        
        [Tooltip("Max Sdk Key")]
        public string maxSdkKey = "-x3h7mcZ5EdJJCd0iDab_rNf-6t9bsentb_ilJcaZ_ORIGB0P4reTeRrMeRe39-EAu-F6Bqcgah9fv-gSdoO1U";
        
        [Header("Max iOS")]
        public string iOSInterstitialAdUnitId;
        public string iOSRewardedAdUnitId;
        public string iOSBannerAdUnitId;

        [Header("Max Android")]
        public string androidInterstitialAdUnitId;
        public string androidRewardedAdUnitId;
        public string androidBannerAdUnitId;
    }
}