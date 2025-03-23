using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game_Screen : MonoBehaviour, Screen_0_Callback
{

    public Text Coins_Indicator;
    public Progress_Bar Progress_Bar;

    public GameObject Rotate_Button;
    public GameObject Next_Button;
    public GameObject Tap_To_Start_Button;



    public void Show(){
        gameObject.SetActive(true);
    }

    public void Hide(){
        gameObject.SetActive(false);
    }

    // UI Actions ---------------------------


    private Action Rotate_Clicked_Callback;
    public void Show_Rotate_Button(Action rotate_clicked){ // Called by Paint_Effect
        Rotate_Clicked_Callback = rotate_clicked;
        Rotate_Button.SetActive(true);
    }
    public void Hide_Rotate_Button(){
       Rotate_Button.SetActive(false);
    }
    public void Rotate_Button_Clicked(){ // Called by button
        Rotate_Clicked_Callback?.Invoke();
        Hide_Rotate_Button();
    }


    private Action Next_Clicked_Callback;

    public void Show_Next_Button(Action next_clicked){ // Called by Paint_Effect
        Next_Clicked_Callback = next_clicked;
        Next_Button.SetActive(true);
    }

    public void Hide_Next_Button(){
       Next_Button.SetActive(false);
    }

    public void Next_Button_Clicked(){ // Called by button
        Next_Clicked_Callback?.Invoke();
        Hide_Next_Button();
    }

    private Action Tap_To_Start_Clicked_Callback;

    public void Show_Start_Button(Action tap_to_start_clicked) {
        Tap_To_Start_Clicked_Callback = tap_to_start_clicked;
        Tap_To_Start_Button.SetActive(true);
    }

    public void Hide_Start_Button(){
       Tap_To_Start_Button.SetActive(false);
    }

    public void Start_Button_Clicked(){ // Called by button
        Tap_To_Start_Clicked_Callback?.Invoke();
        Hide_Start_Button();
    }



    // Game Update Actions -----------------------

    public void Update_Screen(){
        Coins_Indicator.text = GDG.Game_Control.Coins.ToString();
    }

    
}
