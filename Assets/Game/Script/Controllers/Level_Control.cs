using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Control : MonoBehaviour {

    [NonSerialized]
    public Player_Control Player_Control;
    public Transform Level_Elements;

    public Action Level_Completed_Callback;
    public Action Level_Failed_Callback;

    private int Player_Entity_Count = 0;
    private int Enemy_Entity_Count = 0;

    private bool Level_Stopped = false;

    private void Awake() {
        Player_Control = GetComponentInChildren<Player_Control>();
    }

    public void Init_Level(Action level_complete, Action level_failed) { // Called by Game_Control
        Level_Completed_Callback = level_complete;
        Level_Failed_Callback = level_failed;
        Calculate_Entity_Counts();
        Player_Control.Init_Player(Level_Completed, Level_Failed);
    }

    public void Level_Completed() { // Called by Player_Control
        if(Level_Stopped) return;
        Level_Stopped = true;
        Player_Control.Bow_Control.Stop_All();
        Level_Completed_Callback?.Invoke();
        GDG.Game_Elements.Confetti_Control.Play_Confetti();
        GDG.Audio_Control.Play_Success();
        GDG.Audio_Control.Play_Haptic_Success();
    }

    public void Level_Failed() {  // Called by Player_Control
        if(Level_Stopped) return;
        Level_Stopped = true;
        Player_Control.Bow_Control.Stop_All();
        Level_Failed_Callback?.Invoke();
        GDG.Audio_Control.Play_Fail();
        GDG.Audio_Control.Play_Haptic_Failure();
    }

    // -------------------------------------

    private void Calculate_Entity_Counts() {
        Damageable[] entities = GetComponentsInChildren<Damageable>();
        for(int i = 0; i < entities.Length; i++) {
            if(entities[i].Entity_Side == e_Entity_Side.Player) {
                Player_Entity_Count++;
            } else if(entities[i].Entity_Side == e_Entity_Side.Enemy) {
                Enemy_Entity_Count++;
            }
        }
    }

    public void Something_Died(Damageable dead) {
        if(dead.Entity_Side == e_Entity_Side.Player) {
            Player_Entity_Count--;
        } else if(dead.Entity_Side == e_Entity_Side.Enemy) {
            Enemy_Entity_Count--;
        }
        if(Player_Entity_Count <= 0) {
            Level_Failed();
        } else if(Enemy_Entity_Count <= 0) {
            Level_Completed();
        }
    }


}
