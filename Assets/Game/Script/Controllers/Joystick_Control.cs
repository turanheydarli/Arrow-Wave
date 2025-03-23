using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick_Control : MonoBehaviour, Pointer_Down_Listener, Pointer_Up_Listener, Drag_Listener {

    public Vector2 Vector { get; private set; }

    private Vector2 Down_Pos;
    private Vector2 Pointer_Pos;

    [SerializeField]
    private float Dead_Zone = 20f;
    [SerializeField]
    private float Joystick_Scale = 100f;

    private Vector2 Prev_Vector;

    private List<Joystick_Listener> Joystick_Listeners = new List<Joystick_Listener>();

    public void Add_Joystick_Listener(Joystick_Listener listener) {
        Joystick_Listeners.Add(listener);
    }

    public void Remove_Joystick_Listener(Joystick_Listener listener) {
        Joystick_Listeners.Remove(listener);
    }

    // ------------------------------

    public void On_Pointer_Down(Vector2 pos) {
        Down_Pos = pos;
        Pointer_Pos = pos;
    }

    public void On_Pointer_Up(Vector2 pos) {
        Vector = Vector2.zero;
        Send_Joystick_Event();
    }

    public void On_Drag(Vector2 delta) {
        Pointer_Pos += delta;
        Vector = Pointer_Pos - Down_Pos;
        Vector = Apply_Filter(Vector);
        Send_Joystick_Event();
    }

    private void Send_Joystick_Event() {
        if(Vector == Prev_Vector) return;
        Prev_Vector = Vector;
        Vector2 Scaled_Vector = Vector / Joystick_Scale;
        for(int i = 0; i < Joystick_Listeners.Count; i++) {
            Joystick_Listeners[i].On_Joystick(Scaled_Vector);
        }
    }

    // ------------------------------

    private Vector2 Apply_Filter(Vector2 vec) {
        if(vec.sqrMagnitude < Dead_Zone * Dead_Zone) {
            return Vector2.zero;
        } else if(vec.sqrMagnitude > Joystick_Scale * Joystick_Scale) {
            Vector2 cur_vec = vec;
            Vector2 nor_vec = vec.normalized * Joystick_Scale;
            Move_Joystick_Center(cur_vec, nor_vec);
            return nor_vec;
        } else {
            return vec;
        }
    }

    private void Move_Joystick_Center(Vector2 cur_vec, Vector2 nor_vec) {
        Vector2 dif_vec = cur_vec - nor_vec;
        Down_Pos += dif_vec;
    }

}

public interface Joystick_Listener {
    void On_Joystick(Vector2 vector);
}
