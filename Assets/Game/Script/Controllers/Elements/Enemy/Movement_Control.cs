using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movement_Control : MonoBehaviour {

    private Rigidbody Rigid_Body;
    private Character_Control Character_Control;
    private Vector3 Movement_Vector = Vector3.zero;
    private Quaternion Target_Rotation = Quaternion.identity;

    private float Move_Speed = 1.4f;

    private void Awake() {
        Rigid_Body = GetComponent<Rigidbody>();
        Character_Control = GetComponent<Character_Control>();
    }

    public void Set_Movement_Speed(float speed) {
        Move_Speed = speed;
    }

    public void Set_Movement_Vector(Vector2 vector) {
        Movement_Vector.x = vector.x;
        Movement_Vector.z = vector.y;
        Update_Target_Rotation(Movement_Vector);
        Character_Control?.Set_Movement_Vector(Movement_Vector * Move_Speed * 0.2f);
    }

    public void Set_Look_Vector(Vector2 vector) {
        Vector3 look_vector = new Vector3(vector.x, 0f, vector.y);
        Update_Target_Rotation(look_vector);
    }

    private void Update_Target_Rotation(Vector3 vector) {
        if(vector != Vector3.zero) {
            Target_Rotation = Quaternion.LookRotation(vector, Vector3.up);
        }
    }

    // Update -----------------------------

    public void Fixed_Update_Movement_Control() {
        Update_Movement_Velocity();
        Rotate_To_Target();
    }

    // ------------------------------------

    private void Update_Movement_Velocity() {
        float vel_y = Rigid_Body.velocity.y;
        Vector3 vel = Movement_Vector * Move_Speed;
        vel.y = vel_y;
        Rigid_Body.velocity = vel;
    }

    private void Rotate_To_Target() {
        Quaternion rot = Quaternion.RotateTowards(Rigid_Body.rotation, Target_Rotation, Time.fixedDeltaTime * 180f);
        Rigid_Body.MoveRotation(rot);
    }

}
