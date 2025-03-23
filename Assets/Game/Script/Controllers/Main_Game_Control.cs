using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main_Game_Control : MonoBehaviour {

    // =======================================================================
    // Public Variables ======================================================

    public UI_Control UI_Control;
    public Input_Control Input_Control;
    public Audio_Control Audio_Control;
    public Section_Control Section_Control;
    public Game_Control Game_Control;
    public Camera_Control Camera_Control;
    public Game_Elements Game_Elements;
    public Settings Settings;
    public Analytics_Control Analytics_Control;
    public Monetization_Control Monetization_Control;
    public OffScreenIndicator OffScreenIndicator;

    // =======================================================================
    // Public Functions ======================================================

    void Awake() {
        // Globalize controllers
        GDG.MGC = this;
        GDG.Main_Game_Control = this;
        GDG.UI_Control = UI_Control;
        GDG.Input_Control = Input_Control;
        GDG.Section_Control = Section_Control;
        GDG.Game_Control = Game_Control;
        GDG.Camera_Control = Camera_Control;
        GDG.Audio_Control = Audio_Control;
        GDG.Analytics_Control = Analytics_Control;
        GDG.Monetization_Control = Monetization_Control;
        GDG.Game_Elements = Game_Elements;
        GDG.Settings = Settings;
        
        OffScreenIndicator.Init();

    }

    void Start() {
       // Start_Game();
    }

    // Entry point
    public void Start_Game() {
        GDG.Section_Control.Start_Entry_Section();
    }


    // UTILITY FUNCTIONS =====================================================
    // =======================================================================
    // Delay Call
    // Usage: 
    // GDG.MGC.Call_Delay(delegate { Debug.Log("TEST!"); }, 2f);
    // OR
    // GDG.MGC.Call_Delay(() => { Debug.Log("TEST!"); }, 2f);
    // OR
    // GDG.MGC.Call_Delay( Function_Name, 2f);

    public void Call_Delay(Action call_action, float delay) {
        StartCoroutine(Delay_Caller(call_action, delay));
    }

    public void Call_Next_Frame(Action call_action) {
        StartCoroutine(Next_Frame_Caller(call_action));
    }

    private IEnumerator Delay_Caller(Action call_action, float delay) {
        yield return new WaitForSeconds(delay);
        call_action.Invoke();
    }

    private IEnumerator Next_Frame_Caller(Action call_action) {
        yield return new WaitForFixedUpdate();
        call_action.Invoke();
    }


}




