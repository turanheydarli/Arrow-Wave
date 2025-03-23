using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home_Screen : MonoBehaviour, Screen_1_Callback
{

    public Action Play_Callback;
    
    public void Show(Action play_callback){
        Play_Callback = play_callback;
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    // UI Actions ---------------------
    public void Play_Button_Clicked()
    {
        Play_Callback?.Invoke();
    }

}
