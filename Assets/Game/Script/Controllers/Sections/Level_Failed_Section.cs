using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Failed_Section : Section
{

    public override void Start_Section(){
        GDG.UI_Control.Level_Failed_Screen.Show(Next_Clicked);
    }

    public override void End_Section(){
        GDG.UI_Control.Level_Failed_Screen.Hide();
    }

    // Section actions -----------------------------------------------

    private void Next_Clicked(){
        End_Section();
        GDG.Section_Control.Home_Section.Start_Section(); // Home section will restart current level
    }

}
