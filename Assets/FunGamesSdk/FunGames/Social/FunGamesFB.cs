using System;
using System.Collections;
using System.Collections.Generic;
using Facebook.MiniJSON;
using Facebook.Unity;
using FunGames.Sdk.Analytics;
using FunGamesSdk.FunGames.Analytics.Helpers;
using UnityEngine;

public class FunGamesFB
{
    // Start is called on the start of FunGamesSocial
    internal static void Start()
    {
        Debug.Log("Initialize FaceBook");
        FacebookHelpers.Initialize();
    }

    public static void ShareOnFB(FacebookDelegate<IShareResult> ShareCallback, string link = "https://developers.facebook.com/")
    {
        FB.ShareLink(
            new Uri(link),
            callback: ShareCallback
        );
    }

    public static void RecommandApp(FacebookDelegate<IAppRequestResult> RecoCallback, bool OnlyForFriendsWhoPlay = false)
    {
        FunGamesAnalytics.NewDesignEvent("FBRecommandToAfriend", "Open");
        FB.AppRequest(
            "Here is a free gift!",
            null,
            OnlyForFriendsWhoPlay ? new List<object>() { "app_users" } : null,
            null, null, null, null,
            RecoCallback
        );
    }

    public static void LinkToInstagram()
    {
        Application.OpenURL("instagram://user?username=tapnation.io");
        FunGamesAnalytics.NewDesignEvent("InstagramButton");
    }

    public static void LinkToTwitter()
    {
        Application.OpenURL("twitter://user?id=1314209122728857607");
        FunGamesAnalytics.NewDesignEvent("TwitterButton");
    }

    public static void LogInFB(FacebookDelegate<ILoginResult> AuthCallback)
    {
        if (!FB.IsLoggedIn)
        {
            var perms = new List<string>() { "public_profile", "email" };
            FB.LogInWithPublishPermissions(perms, AuthCallback);
        }
    }

    public static void GetFacebookInfo(FacebookDelegate<IGraphResult> callback)
    {
        //FB.API("/me", HttpMethod.GET, printAll);
        FB.API("/me?fields=id,name,email", HttpMethod.GET, callback, new Dictionary<string, string>() { });
        //FB.API("/me/likes", HttpMethod.GET, callback, new Dictionary<string, string>() { });
        //FB.API("/user", HttpMethod.GET, printAll);
    }

    private static void printAll(IResult result)
    {
        if (!string.IsNullOrEmpty(result.Error))
        {
            Debug.Log("Error - Check log for details");
        }
        else if (result.Cancelled)
        {
            Debug.Log("Cancelled - Check log for details");
        }
        else if (!string.IsNullOrEmpty(result.RawResult))
        {
            var dict = Json.Deserialize(result.RawResult) as Dictionary<string, object>;
            //string userName = dict["name"].ToString();
            Debug.Log("Success - Check log for details");
            Debug.Log(result.RawResult);
            //Debug.Log("userName : " + userName);
        }
        else
        {
            Debug.Log("Empty Response");
        }
    }
}
