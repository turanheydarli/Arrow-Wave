using System.Collections;
using System.Collections.Generic;
using FunGames.Sdk.Tools;
using Proyecto26;
using SimpleJSON;
using UnityEngine;

public static class RemoteAPICrosspromo
{
    private static string _remoteConfigApi;
    

    public static string FetchNameOfCreative()
    {
        _remoteConfigApi = "https://api.tnapps.xyz/v1/crosspromo/ml/";
        _remoteConfigApi += Application.identifier;
        var hash = FunGamesApiHelpers.CreateToken(_remoteConfigApi);
        var bitString = FunGamesApiHelpers.GetBitString();
        RestClient.DefaultRequestHeaders["Authorization"] = "HMAC " + bitString + " " + hash;
        RestClient.DefaultRequestHeaders["x-app-build"] = Application.version;
        RestClient.DefaultRequestHeaders["User-Agent"] = SystemInfo.deviceModel;
        
        Debug.Log("_remoteConfigApi : " + _remoteConfigApi);
        RestClient.Get(_remoteConfigApi)
            .Then(response =>
            {
                Debug.Log(response.Text);
                Debug.Log("[Remote Config] Fetch OK");
                JSONNode node = JSON.Parse(response.Text);
                Debug.Log(node.Value);
                return node.Value;
            })
            .Catch(err =>
            {
                Debug.Log("[Remote Config] Fetch KO");
                return "";
            });
        return "";
    }
}
