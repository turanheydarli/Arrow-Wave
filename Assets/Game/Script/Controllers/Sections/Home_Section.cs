using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Home_Section : Section
{

    public override void Start_Section(){
        On_Play_Clicked();
        // GDG.UI_Control.Home_Screen.Show(On_Play_Clicked);
        // GDG.Camera_Control.Set_Home_Camera();
    }

    public override void End_Section(){
        GDG.UI_Control.Home_Screen.Hide();
    }

    // Section actions
    private void On_Play_Clicked(){
        End_Section();
        GDG.Section_Control.Game_Section.Start_Section();
    }

}
