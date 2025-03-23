using System.Collections;
using System.Collections.Generic;
using FunGames.Sdk.Analytics;
using UnityEngine;

public class SocialButton : MonoBehaviour
{
    public void LinkInsta()
    {
        FunGamesFB.LinkToInstagram();
        FunGamesAnalytics.NewDesignEvent("InstagramButton");
    }

    public void LinkTwitter()
    {
        FunGamesFB.LinkToTwitter();
        FunGamesAnalytics.NewDesignEvent("TwitterButton");
    }
}
