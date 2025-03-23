using System;
using System.Collections;
using FunGames.Sdk.Analytics;
using FunGamesSdk;
using FunGamesSdk.FunGames.Ads;
using GameAnalyticsSDK;
using OgurySdk;
using UnityEngine;

public class FunGamesMax
{
	private static string _maxSdkKey;
	private static string _interstitialAdUnitId;
	private static string _rewardedAdUnitId;
	private static string _bannerAdUnitId;

	private static int _interstitialRetryAttempt;
	private static int _rewardedRetryAttempt;
	private static int _bannerRetryAttempt;

	private static Action<string, string, int> _rewardedCallback;
	private static string _rewardedCallbackArgString;
	private static int _rewardedCallbackArgInt;

	private static Action<string, string, int> _interstitialCallback;
	private static string _interstitialCallbackArgString;
	private static int _interstitialCallbackArgInt;

	private static bool _isBannerLoaded;
	private static bool _showBannerAsked;
	private static bool _isBannerShowing;

	// Awake is called on the awake of FunGamesAds
	internal static void Awake()
	{
		var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
		_maxSdkKey = settings.maxSdkKey;

#if UNITY_IOS
		_interstitialAdUnitId = settings.iOSInterstitialAdUnitId;
		_rewardedAdUnitId = settings.iOSRewardedAdUnitId;
		_bannerAdUnitId = settings.iOSBannerAdUnitId;
#endif

#if UNITY_ANDROID
			_interstitialAdUnitId = settings.androidInterstitialAdUnitId;
			_rewardedAdUnitId = settings.androidRewardedAdUnitId;
			_bannerAdUnitId = settings.androidBannerAdUnitId;
#endif
	}

	// Start is called on the start of FunGamesAds
	// Start is called on the start of FunGamesAds
	internal static void Start()
	{
		MaxSdk.SetHasUserConsent(true);
		MaxSdk.SetIsAgeRestrictedUser(false);
		var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");
		if (settings.useMax)
		{
			MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
				if (sdkConfiguration.ConsentDialogState == MaxSdkBase.ConsentDialogState.Applies)
				{
					if (PlayerPrefs.HasKey("GDPRAnswered"))
					{
						MaxSdk.SetHasUserConsent(PlayerPrefs.GetInt("GDPRAnswered") == 1);
						MaxSdk.SetIsAgeRestrictedUser(false);
					}
					else
					{
						MaxSdk.SetHasUserConsent(false);
						MaxSdk.SetIsAgeRestrictedUser(false);
					}
				}
				else
				{
					// No need to show consent dialog, proceed with initialization
					MaxSdk.SetHasUserConsent(true);
					MaxSdk.SetIsAgeRestrictedUser(false);
				}
			};
			//FunGamesMax.Start();
			if (PlayerPrefs.HasKey("GDPRAnswered"))
			{
				MaxSdk.SetHasUserConsent(PlayerPrefs.GetInt("GDPRAnswered") == 1);
				MaxSdk.SetIsAgeRestrictedUser(false);
			}
			else
			{
				MaxSdk.SetHasUserConsent(false);
				MaxSdk.SetIsAgeRestrictedUser(false);
			}

			MaxSdk.SetSdkKey(_maxSdkKey);
			/*MaxSdkCallbacks.OnSdkInitializedEvent += sdkConfiguration => {
				InitializeInterstitialAds();
				InitializeRewardedAds();
				InitializeBannerAds();
			};*/
			MaxSdk.SetUserId(SystemInfo.deviceUniqueIdentifier);
			MaxSdk.InitializeSdk();
			Debug.Log("Initializing FunGamesAds");
		}
	}

	public static void InitializeAds()
    {
		InitializeInterstitialAds();
		InitializeRewardedAds();
		InitializeBannerAds();
	}

	private static void InitializeInterstitialAds()
	{
		MaxSdkCallbacks.OnInterstitialLoadedEvent += OnInterstitialLoadedEvent;
		MaxSdkCallbacks.OnInterstitialLoadFailedEvent += OnInterstitialFailedEvent;
		MaxSdkCallbacks.OnInterstitialAdFailedToDisplayEvent += InterstitialFailedToDisplayEvent;
		MaxSdkCallbacks.OnInterstitialDisplayedEvent += OnInterstitialDisplayedEvent;
		MaxSdkCallbacks.OnInterstitialHiddenEvent += OnInterstitialDismissedEvent;
		try
		{
			LoadInterstitial();
		}
		catch
		{
			Debug.Log("Failed Load Interstitials : Please Check Ad Unit");
		}
	}

	private static void LoadInterstitial()
	{
		MaxSdk.LoadInterstitial(_interstitialAdUnitId);
	}

	private static void OnInterstitialLoadedEvent(string adUnitId)
	{
		_interstitialRetryAttempt = 0;
	}

	private static void OnInterstitialFailedEvent(string adUnitId, int errorCode)
	{
		_interstitialRetryAttempt++;
		var retryDelay = Math.Pow(2, _interstitialRetryAttempt);

        //FunGamesAds._instance.Invoke(nameof(LoadInterstitial), (float)retryDelay);
		FunGamesAds._instance.StartCoroutine(WaitToLoadInter((float)retryDelay));
		FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial);
		_interstitialCallback?.Invoke("fail", _interstitialCallbackArgString, _interstitialCallbackArgInt);
		_interstitialCallback = null;
	}

	static IEnumerator WaitToLoadInter(float time)
	{
		yield return time;
		LoadInterstitial();
	}

	private static void OnInterstitialDisplayedEvent(string adUnitId)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Interstitial);
		_interstitialCallback?.Invoke("success", _interstitialCallbackArgString, _interstitialCallbackArgInt);
		_interstitialCallback = null;
	}

	private static void InterstitialFailedToDisplayEvent(string adUnitId, int errorCode)
	{
		LoadInterstitial();
		FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Interstitial);
		_interstitialCallback?.Invoke("fail", _interstitialCallbackArgString, _interstitialCallbackArgInt);
		_interstitialCallback = null;
	}

	private static void OnInterstitialDismissedEvent(string adUnitId)
	{
		LoadInterstitial();
		FunGamesAnalytics.NewDesignEvent("Interstitial", "Dismissed");
	}

	private static void InitializeRewardedAds()
	{
		MaxSdkCallbacks.OnRewardedAdLoadedEvent += OnRewardedAdLoadedEvent;
		MaxSdkCallbacks.OnRewardedAdLoadFailedEvent += OnRewardedAdFailedEvent;
		MaxSdkCallbacks.OnRewardedAdFailedToDisplayEvent += OnRewardedAdFailedToDisplayEvent;
		MaxSdkCallbacks.OnRewardedAdDisplayedEvent += OnRewardedAdDisplayedEvent;
		MaxSdkCallbacks.OnRewardedAdClickedEvent += OnRewardedAdClickedEvent;
		MaxSdkCallbacks.OnRewardedAdHiddenEvent += OnRewardedAdDismissedEvent;
		MaxSdkCallbacks.OnRewardedAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;

		try
		{
			LoadRewardedAd();
		}
		catch
		{
			Debug.Log("Failed Load Rewarded : Please Check Ad Unit");
		}
	}

	private static void LoadRewardedAd()
	{
		MaxSdk.LoadRewardedAd(_rewardedAdUnitId);
	}

	private static void OnRewardedAdLoadedEvent(string adUnitId)
	{
		_rewardedRetryAttempt = 0;
		FunGamesAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.RewardedVideo);
	}

	private static void OnRewardedAdFailedEvent(string adUnitId, int errorCode)
	{
		_rewardedRetryAttempt++;
		var retryDelay = Math.Pow(2, _rewardedRetryAttempt);

		//FunGamesAds._instance.Invoke("LoadRewardedAd", (float)retryDelay);
		FunGamesAds._instance.StartCoroutine(WaitToLoadRewarded((float)retryDelay));
		FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo);
		_rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
		_rewardedCallback = null;
	}

	static IEnumerator WaitToLoadRewarded(float time)
    {
		yield return time;
		LoadRewardedAd();
    }

	private static void OnRewardedAdDisplayedEvent(string adUnitId)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.RewardedVideo);
	}

	private static void OnRewardedAdFailedToDisplayEvent(string adUnitId, int errorCode)
	{
		LoadRewardedAd();
		FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.RewardedVideo);
		_rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
		_rewardedCallback = null;
	}

	private static void OnRewardedAdClickedEvent(string adUnitId)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.RewardedVideo);
	}

	private static void OnRewardedAdDismissedEvent(string adUnitId)
	{
		LoadRewardedAd();
		_rewardedCallback?.Invoke("fail", _rewardedCallbackArgString, _rewardedCallbackArgInt);
		_rewardedCallback = null;
	}

	private static void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.RewardReceived, GAAdType.RewardedVideo);
		_rewardedCallback?.Invoke("success", _rewardedCallbackArgString, _rewardedCallbackArgInt);
		_rewardedCallback = null;
	}

	private static void InitializeBannerAds()
	{
		MaxSdkCallbacks.OnBannerAdLoadedEvent += BannerIsLoaded;
		MaxSdkCallbacks.OnBannerAdLoadFailedEvent += OnBannerAdFailedEvent;
		MaxSdkCallbacks.OnBannerAdClickedEvent += OnBannerAdClickedEvent;
		MaxSdkCallbacks.OnBannerAdExpandedEvent += OnRewardedAdExpandedEvent;

		try
		{
			MaxSdk.CreateBanner(_bannerAdUnitId, MaxSdkBase.BannerPosition.BottomCenter);
			MaxSdk.SetBannerBackgroundColor(_bannerAdUnitId, Color.black);
			MaxSdk.SetBannerExtraParameter(_bannerAdUnitId, "adaptive_banner", "false");
		}
		catch
		{
			Debug.Log("Failed Create Banner : Please Check Ad Unit");
		}
	}

	internal static void BannerIsLoaded(string adUnitId)
	{
		_isBannerLoaded = true;
		FunGamesAnalytics.NewAdEvent(GAAdAction.Loaded, GAAdType.Banner);
		if(_isBannerShowing)
			FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Banner);
		if (_showBannerAsked)
		{
			ShowBannerAd();
		}
	}

	private static void OnBannerAdFailedEvent(string adUnitId, int errorCode)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.FailedShow, GAAdType.Banner);
	}

	private static void OnBannerAdClickedEvent(string adUnitId)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.Clicked, GAAdType.Banner);
	}

	private static void OnRewardedAdExpandedEvent(string adUnitId)
	{
		FunGamesAnalytics.NewAdEvent(GAAdAction.Undefined, GAAdType.Banner);
	}

	internal static void ShowBannerAd()
	{
		if (_isBannerShowing)
		{
			return;
		}

		if (_showBannerAsked == false)
		{
			_showBannerAsked = true;
		}

		if (_isBannerLoaded == false)
		{
			return;
		}

		MaxSdk.ShowBanner(_bannerAdUnitId);
		_isBannerShowing = true;
		FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Banner);
	}

	internal static void HideBannerAd()
	{
		if (_isBannerLoaded == false)
		{
			return;
		}

		MaxSdk.HideBanner(_bannerAdUnitId);
		_isBannerShowing = false;
		_showBannerAsked = false;
	}

	internal static bool IsRewardedAdReady()
	{
		return MaxSdk.IsRewardedAdReady(_rewardedAdUnitId);
	}

	internal static void LoadAds()
	{
		ShowBannerAd();
		LoadRewardedAd();
		LoadInterstitial();
	}

	internal static void ShowRewarded(Action<string, string, int> callback, string callbackArgsString = "", int callbackArgsInt = 0)
	{
		_rewardedCallback += callback;
		_rewardedCallbackArgString = callbackArgsString;
		_rewardedCallbackArgInt = callbackArgsInt;

		if (MaxSdk.IsRewardedAdReady(_rewardedAdUnitId))
		{
			try
			{
				MaxSdk.ShowRewardedAd(_rewardedAdUnitId);
				FunGamesAnalytics.NewDesignEvent("Rewarded" + callbackArgsString, "succeeded");
				callback?.Invoke("succeeded", callbackArgsString, callbackArgsInt);
			}
			catch (Exception e)
			{
				callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
				FunGamesAnalytics.NewDesignEvent("RewardedError" + callbackArgsString, "UserQuitBeforeEndingAd");
				Debug.Log(e);
				throw;
			}
		}
		else
		{
			callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
			FunGamesAnalytics.NewDesignEvent("RewardedNoAd" + callbackArgsString, "NoAdReady");
			_rewardedCallback = null;
		}
	}

	internal static void ShowInterstitial(Action<string, string, int> callback, string callbackArgsString = "", int callbackArgsInt = 0)
	{
		_interstitialCallback += callback;
		_interstitialCallbackArgString = callbackArgsString;
		_interstitialCallbackArgInt = callbackArgsInt;

		if (MaxSdk.IsInterstitialReady(_interstitialAdUnitId))
		{
			try
			{
				MaxSdk.ShowInterstitial(_interstitialAdUnitId);
			}
			catch (Exception e)
			{
				callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
				FunGamesAnalytics.NewDesignEvent("Error", "UserQuitBeforeEndingAd");
				Debug.Log(e);
				throw;
			}
		}
		else
		{
			callback?.Invoke("fail", callbackArgsString, callbackArgsInt);
			_interstitialCallback = null;
		}
	}

	void InterstitialCallbackFunc(string status, string argString, int argInt)
    {
		FunGamesAnalytics.NewDesignEvent("InterstitialComplete");
	}
}