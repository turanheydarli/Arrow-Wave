using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Level_Section : MonoBehaviour
{
    public Action Section_Completed_Callback;

    private bool Started_Section = false;

    private void Awake() {
        if(!Started_Section){
            gameObject.SetActive(false);
        }
    }

    public virtual void Start_Section(Action section_completed){
        Section_Completed_Callback = section_completed;
        Started_Section = true;
        gameObject.SetActive(true);
        Start_Section(); // Internal
        // GDG.Analytics_Control.Send_Level_Section_Start();
    }

    protected abstract void Start_Section();

    protected virtual void End_Section(){
        // GDG.Analytics_Control.Send_Level_Section_Complete();
        Section_Completed_Callback?.Invoke();
    }

    public abstract void On_Pointer_Down(Vector2 pos);

    public abstract void On_Drag(Vector2 pos);

    public abstract void On_Pointer_Up(Vector2 pos);


}
