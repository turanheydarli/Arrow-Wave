using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Control : MonoBehaviour
{

    public bool Input_Enabled=true;

    public List<Pointer_Down_Listener> Pointer_Down_Listeners;
    public List<Pointer_Up_Listener> Pointer_Up_Listeners;
    public List<Drag_Listener> Drag_Listeners;
    
    [HideInInspector]
    public Joystick_Control Joystick_Control;

    private Vector2 Pointer_Position;

    // Init =====================================================================

    private void Awake() {
        Pointer_Down_Listeners = new List<Pointer_Down_Listener>();
        Pointer_Up_Listeners = new List<Pointer_Up_Listener>();
        Drag_Listeners = new List<Drag_Listener>();

        Joystick_Control = GetComponent<Joystick_Control>();
        if(Joystick_Control != null){
            Add_Pointer_Down_Listener(Joystick_Control);
            Add_Pointer_Up_Listener(Joystick_Control);
            Add_Drag_Listener(Joystick_Control);
        }
    }

    // Enable/Disable ===========================================================

    public void Enable_Input(){
        Input_Enabled = true;
    }

    public void Disable_Input(){
        On_Pointer_Up(Pointer_Position);
        Input_Enabled = false;
    }

    // Add/Remove Listeners =====================================================

    // Add

    public void Add_Pointer_Down_Listener(Pointer_Down_Listener listener){
        Pointer_Down_Listeners.Add(listener);
    }
    
    public void Add_Pointer_Up_Listener(Pointer_Up_Listener listener){
        Pointer_Up_Listeners.Add(listener);
    }

    public void Add_Drag_Listener(Drag_Listener listener){
        Drag_Listeners.Add(listener);
    }

    // Remove 
    
    public void Remove_Pointer_Down_Listener(Pointer_Down_Listener listener){
        Pointer_Down_Listeners.Remove(listener);
    }
    
    public void Remove_Pointer_Up_Listener(Pointer_Up_Listener listener){
        Pointer_Up_Listeners.Remove(listener);
    }

    public void Remove_Drag_Listener(Drag_Listener listener){
        Drag_Listeners.Remove(listener);
    }

    // Input Actions ===========================================================

    public void On_Pointer_Down(Vector2 pos){
        if(!Input_Enabled){return;}
        Pointer_Position = pos;
        for (int i = 0; i < Pointer_Down_Listeners.Count; i++){
            Pointer_Down_Listeners[i].On_Pointer_Down(pos);
        }
    }

    public void On_Pointer_Up(Vector2 pos){
        if(!Input_Enabled){return;}
        for (int i = 0; i < Pointer_Up_Listeners.Count; i++){
            Pointer_Up_Listeners[i].On_Pointer_Up(pos);
        }
    }

    public void On_Drag(Vector2 delta){
        if(!Input_Enabled){return;}
        Pointer_Position += delta;
        for (int i = 0; i < Drag_Listeners.Count; i++){
            Drag_Listeners[i].On_Drag(delta);
        }
    }
    
}

public interface Pointer_Down_Listener{
    void On_Pointer_Down(Vector2 pos);
}
public interface Pointer_Up_Listener{
    void On_Pointer_Up(Vector2 pos);
}
public interface Drag_Listener{
    void On_Drag(Vector2 delta);
}