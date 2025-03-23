using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanelRate : MonoBehaviour
{
    public static PanelRate Instance;

    public string UrlAndroid;
    public string UrlIos;
    public GameObject Parent;
    public List<Transform> MyStars;
    public int NbrStars;
    public GameObject BtnSure, BtnLater,Thanks;

    void Awake()
    {
        Instance = this;
    }

    public void Open()
    {
        if(PlayerPrefs.GetInt("RateQG",0)==0)
            Parent.SetActive(true);
    }

    public void Close()
    {
        Parent.SetActive(false);
    }

    public void Rate(int nbr)
    {
        NbrStars = nbr;
        foreach(Transform t in MyStars)
        {
            t.GetChild(0).gameObject.SetActive(false);
        }

        for(int i = 0; i < nbr; i++)
        {
           MyStars[i].GetChild(0).gameObject.SetActive(true);
        }
    }

    public void Sure()
    {
        if (NbrStars >= 3)
        {
            OpenLink();
            Close();
        }
        else
        {
            StartCoroutine(Close(1));       
        }
        PlayerPrefs.SetInt("RateQG", 1);
    }

    public IEnumerator Close(float time)
    {
        Thanks.SetActive(true);
        BtnSure.SetActive(false);
        BtnLater.SetActive(false);
        yield return new WaitForSeconds(time);
        Close();
        BtnSure.SetActive(true);
        BtnLater.SetActive(true);
        Thanks.SetActive(false);
    }


    public void OpenLink()
    {
#if UNITY_IOS
     Application.OpenURL(UrlIos);
#elif UNITY_ANDROID
        Application.OpenURL(UrlAndroid);
#else
    Application.OpenURL(UrlAndroid);
#endif


    }
}
