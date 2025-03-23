using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Set_Canvas_Event_Camera : MonoBehaviour {

    private void Awake() {
        Canvas can = GetComponent<Canvas>();
        can.worldCamera = GDG.Camera_Control.Camera;
    }

}
