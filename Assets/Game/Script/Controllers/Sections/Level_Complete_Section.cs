using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Complete_Section : Section
{

    public override void Start_Section(){
        GDG.UI_Control.Level_Completed_Screen.Show(Next_Clicked);
    }

    public override void End_Section(){
        GDG.UI_Control.Level_Completed_Screen.Hide();
    }

    // Section actions -----------------------------------------------

    private void Next_Clicked(){
        GDG.Game_Control.Finish_Current_Level(); // Increments Current_Level_Index
        End_Section();
        GDG.Section_Control.Home_Section.Start_Section();
    }

}
