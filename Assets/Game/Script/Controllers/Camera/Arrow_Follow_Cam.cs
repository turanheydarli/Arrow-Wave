using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow_Follow_Cam : MonoBehaviour {

    private float Follow_Distance = 3.8f;

    private bool Follow_Active = true;

    private Vector3 Target_Position;
    private Quaternion Target_Rotation;

    public void Activate() {
        Follow_Active = true;
        Set_Follow_Target_Immediate();
    }

    public void Deactivate() {
        Follow_Active = false;
    }

    private void FixedUpdate() {
        if(Follow_Active && GDG.Game_Control.Playing && GDG.Player_Control.Bow_Control.Has_Active_Arrows()) {
            Vector3 arrow_direction = GDG.Player_Control.Bow_Control.Get_Active_Arrows_Direction();
            Target_Position = GDG.Player_Control.Bow_Control.Get_Active_Arrows_Center() + arrow_direction * Follow_Distance * -1f;
            Target_Rotation = Quaternion.LookRotation(arrow_direction);
            Target_Position += (Target_Rotation * Vector3.up) * 2f;
            Target_Rotation = Target_Rotation * Quaternion.Euler(new Vector3(20f, 0f, 0f));

            transform.position = Vector3.Lerp(transform.position, Target_Position, 12f * Time.fixedDeltaTime);
            transform.rotation = Quaternion.Lerp(transform.rotation, Target_Rotation, 20f * Time.fixedDeltaTime);
        }
    }

    private void Set_Follow_Target_Immediate() {
        if(Follow_Active && GDG.Game_Control.Playing && GDG.Player_Control.Bow_Control.Has_Active_Arrows()) {
            Vector3 arrow_direction = GDG.Player_Control.Bow_Control.Get_Active_Arrows_Direction();
            transform.position = GDG.Player_Control.Bow_Control.Get_Active_Arrows_Center() + arrow_direction * Follow_Distance * -1f;
            transform.rotation = Quaternion.LookRotation(arrow_direction);
        }
    }

}
