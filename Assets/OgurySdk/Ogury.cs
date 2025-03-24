using System;
using System.Runtime.InteropServices;
using UnityEngine;

namespace OgurySdk
{
    public class Ogury
    {
#if UNITY_ANDROID
        private static AndroidJavaObject ogury = new AndroidJavaObject("com.ogury.unity.Ogury");
#endif

#if UNITY_IOS
        [DllImport("__Internal", CharSet = CharSet.Ansi)]
        private static extern int ogury_startWithAssetKey(string assetKey);

        [DllImport("__Internal")]
        private static extern string ogury_getSdkVersion();
#endif

        public static void Start(string androidAssetKey, string iosAssetKey)
        {
#if UNITY_EDITOR
            // Noop
#elif UNITY_ANDROID
            ogury.CallStatic("start", androidAssetKey);
            Debug.Log("Android SDK Version : " + SdkVersion);
#elif UNITY_IOS
            ogury_startWithAssetKey(iosAssetKey);
            Debug.Log("iOS SDK Version : " + SdkVersion);
#endif
        }

        [Obsolete("StartAds is deprecated, please use Start(androidAssetKey, iosAssetKey) instead.")]
        public static void StartAds()
        {
#if UNITY_EDITOR
            // Noop
#elif UNITY_ANDROID
            // Noop
#elif UNITY_IOS
           // Noop
#endif
        }

        public static string SdkVersion
        {
            get
            {
#if UNITY_EDITOR
                return "0.0.0";
#elif UNITY_ANDROID
                return ogury.CallStatic<string>("getSdkVersion");
#elif UNITY_IOS
                return ogury_getSdkVersion();
#endif
            }
        }
    }
}