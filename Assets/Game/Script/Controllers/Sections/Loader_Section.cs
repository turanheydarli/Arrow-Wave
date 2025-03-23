using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Loader_Section : Section
{
    public override void Start_Section(){
        GDG.UI_Control.Loading_Screen.Show(0);
        GDG.Game_Control.Load_Game_State();
        End_Section();
    }

    public override void End_Section(){
        StartCoroutine(Hide_Loader());
    }

    IEnumerator Hide_Loader(){
        yield return new WaitForSeconds(0.1f);
        GDG.UI_Control.Loading_Screen.Hide();
        GDG.Section_Control.Home_Section.Start_Section();
    }
}
