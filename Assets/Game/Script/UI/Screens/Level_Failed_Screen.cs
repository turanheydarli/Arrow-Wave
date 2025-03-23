using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level_Failed_Screen : MonoBehaviour, Screen_1_Callback
{
    Action Next_Callback;
   
    public void Show(Action next_callback){
        Next_Callback = next_callback;
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    public void Next_Clicked(){
        Next_Callback?.Invoke();
    }

}
