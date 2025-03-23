using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Section : Section {
    // Dependencies could be injected as serialized fields
    // Pointer event could be registered as listener to pass


    public override void Start_Section() {
        StartCoroutine(Level_Start_Sequence());
    }

    public override void End_Section() {
        GDG.UI_Control.Game_Screen.Hide();
    }

    // -------------------------------------

    private IEnumerator Level_Start_Sequence() {
        GDG.Input_Control.Disable_Input();
        // GDG.UI_Control.Loading_Screen.Show();
        yield return new WaitForSeconds(GDG.UI_Control.Loading_Screen.Fade_Duration);
        GDG.Game_Control.Start_Level(On_Level_Ready, On_Level_Completed, On_Level_Failed);
    }


    private void On_Level_Ready() {
        // GDG.UI_Control.Loading_Screen.Hide();
        GDG.Camera_Control.Set_Bow_Cam();
        GDG.UI_Control.Game_Screen.Show();
        GDG.Input_Control.Enable_Input();
        GDG.Game_Control.Play_Level();
    }

    public void On_Level_Completed() {
        GDG.Input_Control.Disable_Input();
        End_Section();
        GDG.Section_Control.Level_Complete_Section.Start_Section();
    }

    public void On_Level_Failed() {
        End_Section();
        GDG.Section_Control.Level_Failed_Section.Start_Section();
    }

    // Util -----------------------------------------------

    public void Load_Next_Level() {
        End_Section();
        GDG.Game_Control.Current_Level_Index++;
        Start_Section();
    }

    public void Load_Prev_Level() {
        End_Section();
        GDG.Game_Control.Current_Level_Index--;
        if(GDG.Game_Control.Current_Level_Index < 0) {
            GDG.Game_Control.Current_Level_Index = 0;
        }
        Start_Section();
    }



}
