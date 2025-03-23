using FunGames.Sdk.Analytics;
using FunGamesSdk;
using GameAnalyticsSDK;
using OgurySdk;
using UnityEngine;

public class FunGamesThumbail
{
    private static OguryThumbnailAd _thumbnailAd;

    private static bool _isThumbnailLoaded;
    private static bool _showThumbnailAsked;

    // Start is called on the start of FunGamesAds
    internal static void Start()
    {
        var settings = Resources.Load<FunGamesSettings>("FunGamesSettings");

        if (settings.useOgury)
        {
            _thumbnailAd = new OguryThumbnailAd(settings.androidThumbnailAdUnitId, settings.iOSThumbnailAdUnitId);
            InitializeThumbnailAd();
			//FungamesOguryChoiceManager.Start();
        }
    }

    private void OnThumbnailComplete(OguryChoiceManager.Answer answer)
    {
        Debug.Log("OnThumbnailComplete");
        _thumbnailAd.Load();
    }

	internal static void InitializeThumbnailAd()
	{
		_thumbnailAd.OnAdLoaded += ThumbnailIsLoaded;
		_thumbnailAd.OnAdClosed += ThumbnailLoad;
		Debug.Log("First load thumbnail");
		_thumbnailAd.Load();
	}

	private static void ThumbnailLoad(OguryThumbnailAd ad)
	{
		Debug.Log("ThumbnailLoad");

		_showThumbnailAsked = false;
		_thumbnailAd.Load();

	}
	static bool isPlaced = false;
	static OguryThumbnailAdGravity gravity;
	static int a;
	static int b;
	private static void ThumbnailIsLoaded(OguryThumbnailAd ad)
	{
		_isThumbnailLoaded = true;
		Debug.Log("_isThumbnailLoaded is set to " + _isThumbnailLoaded);
		if (_showThumbnailAsked)
		{
			if(isPlaced)
				ShowThumbnailAd(gravity,a,b);
			else
				ShowThumbnailAd();
		}
	}
	
	internal static void ShowThumbnailAd()
	{
		if (_showThumbnailAsked == false)
		{
			_showThumbnailAsked = true;
		}
		//_isThumbnailLoaded = true;
		Debug.Log("Thumbnail satus show : " + _showThumbnailAsked + " _isThumbnailLoaded : " + _isThumbnailLoaded);
		if (_isThumbnailLoaded == false)
		{
			return;
		}
		_thumbnailAd.Show();
		FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Undefined);
	}

	internal static void ShowThumbnailAd(OguryThumbnailAdGravity g,int x, int y)
	{
		isPlaced = true;
		gravity = g;
		a = x;
		b = y;
		if (_showThumbnailAsked == false)
		{
			_showThumbnailAsked = true;
		}
		//_isThumbnailLoaded = true;
		Debug.Log("Thumbnail satus show : " + _showThumbnailAsked + " _isThumbnailLoaded : " + _isThumbnailLoaded);
		if (_isThumbnailLoaded == false)
		{
			return;
		}
		_thumbnailAd.Show(g,x,y);
		FunGamesAnalytics.NewAdEvent(GAAdAction.Show, GAAdType.Undefined);
	}
}
