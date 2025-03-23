using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Control : MonoBehaviour {


    public Action Level_Completed_Callback;
    public Action Level_Failed_Callback;

    public Aim_Control Aim_Control;
    public Bow_Control Bow_Control;

    public Camera Bow_Camera;


    // Awake --------------------------
    // Remove listeners if not needed!

    // private void Awake() {
    //     GDG.Game_Control.Add_Update_Listener(Update_Player_Control);
    //     GDG.Game_Control.Add_Fixed_Update_Listener(Fixed_Update_Player_Control);
    // }

    // private void OnDestroy() {
    //     GDG.Game_Control.Remove_Update_Listener(Update_Player_Control);
    //     GDG.Game_Control.Remove_Fixed_Update_Listener(Fixed_Update_Player_Control);
    // }

    // Init ---------------------------

    public void Init_Player(Action level_complete, Action level_failed) {
        Level_Completed_Callback = level_complete;
        Level_Failed_Callback = level_failed;
        Start_Player();
    }

    // Sections -------------------------

    private void Start_Player() {
        // GDG.MGC.Call_Delay(Level_Failed, 1f);
        GDG.MGC.Call_Delay(() => { GDG.UI_Control.Tutorial_Elements.Show_Aim_Tutorial(); }, 1f);

    }

    // Gameplay ----------------------------

    public void On_Pointer_Down(Vector2 pos) {
        Bow_Control.On_Pointer_Down(pos);
    }

    public void On_Drag(Vector2 delta) {
        Bow_Control.On_Drag(delta);
    }

    public void On_Pointer_Up(Vector2 pos) {
        Bow_Control.On_Pointer_Up(pos);
    }

    // Update --------------------------------
    // Called by Game_Control on each frame

    // public void Update_Player_Control() {

    // }

    // public void Fixed_Update_Player_Control() {

    // }

    // Callbacks --------------------------

    private void Level_Completed() {
        Level_Completed_Callback?.Invoke();
    }

    private void Level_Failed() {
        Level_Failed_Callback?.Invoke();
    }


}

