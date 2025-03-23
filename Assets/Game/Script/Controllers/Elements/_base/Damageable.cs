using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Damageable : MonoBehaviour {

    public float Health = 100f;

    public bool Can_Arrow_Stab = true;
    public e_Entity_Side Entity_Side = e_Entity_Side.None;
    protected bool Dead = false;


    // ----------------------

    public virtual void Apply_Damage(Damage damage) {
        if(damage.Attacker_Side != Entity_Side) {
            Health -= damage.Intensity;
            if(Health <= 0f) {
                Trigger_Die();
            }
        }
    }

    private void Trigger_Die() {
        if(Dead) { return; }
        Dead = true;
        Die();
        GDG.Level_Control.Something_Died(this);
    }

    protected virtual void Die() { }

    public virtual bool Is_Alive() {
        return !Dead;
    }

    // ----------------------

    private List<Action> Destroy_Callback_List = new List<Action>();

    public void Add_Destroy_Listener(Action destroy_callback) {
        Destroy_Callback_List.Add(destroy_callback);
    }

    private void OnDestroy() {
        for(int i = 0; i < Destroy_Callback_List.Count; i++) {
            Destroy_Callback_List[i]?.Invoke();
        }
    }

}
