using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using FunGames.Sdk.Analytics;
using TMPro;
using UnityEngine;
#if UNITY_IOS
using UnityEngine.iOS;
#endif
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Video;
using FunGames.Sdk.Ads.Crosspromo;
using Random = UnityEngine.Random;
using com.adjust.sdk;

namespace FunGamesSdk.FunGames.Ads.Crosspromo.Scripts
{
    public class CrosspromoLocalController : MonoBehaviour
    {
        public static CrosspromoLocalController instance;
        
        public List<VideoClip> clips;
        public List<string> titles;
        //public List<string> descriptions;
        public List<string> promotedUrls;
        public List<string> promotedStoreIds;
        public List<string> Ids;

        private VideoPlayer _videoPlayer;
        private TextMeshProUGUI _videoTitle;
        private TextMeshProUGUI _videoDescription;
        private Button _videoButton;
        private Button _playButton;

        private string _promotedUrl;
        private string _promotedId;

        private int index = -1;

        private Coroutine _videoPlayingCoroutine;

        private void Awake ()
        {
            instance = this;

            _videoPlayer = transform.Find("Root/Video").GetComponent<VideoPlayer>();
            _videoTitle = transform.Find("Root/Title").GetComponent<TextMeshProUGUI>();
            //_videoDescription = transform.Find("Root/Description").GetComponentInChildren<TextMeshProUGUI>();
            _videoButton = transform.Find("Root/Video").GetComponent<Button>();
            _playButton = transform.Find("Root/PlayButton").GetComponent<Button>();
            
            if (_videoPlayer != null)
                _videoPlayer.Stop ();
        }

        public void Start()
        {
            PlayVideo();
        }
        public void PlayVideo()
        {
            PlayVideo(PlayVideo);
        }

        public void PlayVideo(Action complete)
        {
            _videoButton.onClick.RemoveAllListeners();
            _playButton.onClick.RemoveAllListeners();
            
            _videoButton.onClick.AddListener(Redirect);
            _playButton.onClick.AddListener(Redirect);
            
            if (_videoPlayer == null)
            {
                Debug.LogError ("videoPlayer == null");
                return;
            }
            if (clips.Count == 0)
            {
                Debug.Log("Nothing to play");
                return;
            }

            // Get the name of the creative from the API
            index = GetIndexRemotely();
            // A random ads is showing
            //index = Random.Range(0, clips.Count);
            // Switch between each ads
            //index = (index + 1) % clips.Count;
            FunGamesAnalytics.NewDesignEvent("CrosspromoPlay", clips[index].name);
            _promotedUrl = promotedUrls[index];
            _promotedId = promotedStoreIds[index];

            SetTitle(titles[index]);
            //SetDescription(descriptions[index]);
            StartPlayingVideo(clips[index], complete);
        }

        private int GetIndexRemotely()
        {
            string clipName = RemoteAPICrosspromo.FetchNameOfCreative();
            int i = 0;
            while(i < clips.Count)
            {
                if (clipName == clips[i].name && promotedUrls[i] != Application.identifier)
                    return i;
                i = i + 1;
            }
            i = Random.Range(0, clips.Count);
            if (promotedUrls[i] == Application.identifier)
                i = (i + Random.Range(1, clips.Count)) % clips.Count;
            return i;
        }

        private void SetTitle(string title)
        {
            _videoTitle.text = title;
        }

        /*private void SetDescription(string description)
        {
            _videoDescription.text = description;
        }*/
        
        private void StartPlayingVideo (VideoClip clip, Action complete)
        {
            _videoPlayer.clip = clip;
            _videoPlayer.Play();

            if (_videoPlayingCoroutine != null)
                StopCoroutine (_videoPlayingCoroutine);

            _videoPlayingCoroutine = StartCoroutine( CheckVideoFinish(complete) );
        }
        
        private IEnumerator CheckVideoFinish (Action complete)
        {
            while (true)
            {
                if (_videoPlayer.isPaused)
                {
                    complete?.Invoke ();
                    yield break;
                }

                yield return null;
            }
        }

        private void Redirect()
        {
            FunGamesAnalytics.NewDesignEvent("crossPromo", "click:" + _promotedUrl);

            //Application.OpenURL(_promotedUrl);
            if (!Application.isEditor || true)
            {
                string url = null;

#if UNITY_IOS
                url = "https://s2s.adjust.com/" + Ids[index] + "?campaign="+ Application.productName + "&adgroup="+ titles[index].Replace(" ", "") + "&creative="+ clips[index].name + "&idfa=" + Device.advertisingIdentifier + "&idfv=" + Device.vendorIdentifier + "&gps_adid=&s2s=1";
                FunGamesAnalytics.NewDesignEvent("CrosspromoPlayClick", clips[index].name);
                Debug.Log(url);
                UnityWebRequest.Get(url).SendWebRequest();
                AppstoreHandler.Instance.openAppInStore(_promotedId);
#endif

#if UNITY_ANDROID
                Adjust.getGoogleAdId((string googleAdId) => {
                        string urlAndroid = "https://s2s.adjust.com/" + Ids[index] + "?campaign=" + Application.productName + "&adgroup=" + titles[index].Replace(" ", "") + "&creative=" + clips[index].name + "&idfa=&idfv=&gps_adid=" + googleAdId + "&s2s=1";
                        AppstoreHandler.Instance.openAppInStore(_promotedUrl);
                        FunGamesAnalytics.NewDesignEvent("CrosspromoPlayClick", clips[index].name);
                        Debug.Log(urlAndroid);
                        UnityWebRequest.Get(urlAndroid).SendWebRequest();
                    }
                );
                AppstoreHandler.Instance.openAppInStore(_promotedUrl);
#endif

            }
            else
            {
                Debug.Log("AppstoreTestScene:: Cannot view app in Editor.");
            }
        }
    }
}