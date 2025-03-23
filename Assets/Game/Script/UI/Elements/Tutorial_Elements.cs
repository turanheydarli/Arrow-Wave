using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_Elements : MonoBehaviour {

    public GameObject Aim_Tutorial;
    public GameObject Control_Tutorial;


    public void Show_Aim_Animation() {
        Aim_Tutorial.SetActive(true);
    }

    public void Hide_Aim_Animation() {
        Aim_Tutorial.SetActive(false);
    }

    public void Show_Control_Animation() {
        Control_Tutorial.SetActive(true);
    }

    public void Hide_Control_Animation() {
        Control_Tutorial.SetActive(false);
    }

    // ----------------------------

    public void Show_Aim_Tutorial() {
        if(Is_Tutorial_Complete()) return;
        Show_Aim_Animation();
    }

    public void Hide_Aim_Tutorial() {
        GDG.MGC.Call_Delay(Hide_Aim_Animation, 1f);
    }

    public void Show_Control_Tutorial() {
        if(Is_Tutorial_Complete()) return;
        Show_Control_Animation();
    }

    public void Hide_Control_Tutorial() {
        GDG.MGC.Call_Delay(Hide_Control_Animation, 0.25f);
        if(GDG.Game_Control.Current_Level_Index > 2) {
            Tutorial_Complete();
        }
    }

    // ----------------------------

    private void Tutorial_Complete() {
        PlayerPrefs.SetInt("Tutorial", 1);
    }

    private bool Is_Tutorial_Complete() {
        if(PlayerPrefs.GetInt("Tutorial") == 1) {
            return true;
        } else {
            return false;
        }
    }






}
