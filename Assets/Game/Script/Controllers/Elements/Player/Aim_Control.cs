using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Aim_Control : MonoBehaviour {

    public Transform X_Pivot; // Aim_Pivot
    public Transform Y_Pivot; // Player_Control

    private float X_Sens = 0.28f;
    private float Y_Sens = 0.28f;

    private float X_Axis = 0f;


    public void On_Drag(Vector2 delta) {

        Vector3 x_piv = X_Pivot.localEulerAngles;
        Vector3 y_piv = Y_Pivot.localEulerAngles;

        y_piv.y += delta.x * Y_Sens;

        X_Axis -= delta.y * X_Sens;
        Limit_X_Axis();
        x_piv.x = X_Axis;

        X_Pivot.localEulerAngles = x_piv;
        Y_Pivot.localEulerAngles = y_piv;

    }


    public void Aim_To_Target(Vector3 target) {

        Y_Pivot.rotation = Quaternion.LookRotation(target - Y_Pivot.position, Vector3.up);
        Vector3 angles = Y_Pivot.localEulerAngles;
        angles.x = 0f;
        Y_Pivot.localEulerAngles = angles;
        X_Pivot.rotation = Quaternion.LookRotation(target - Y_Pivot.position);
        Vector3 x_piv = X_Pivot.localEulerAngles;
        X_Axis = x_piv.x;
        Limit_X_Axis();
        x_piv.x = X_Axis;
        X_Pivot.localEulerAngles = x_piv;
    }

    private void Limit_X_Axis() {
        X_Axis = Mathf.Clamp(X_Axis, -45f, 85f);
    }

}
