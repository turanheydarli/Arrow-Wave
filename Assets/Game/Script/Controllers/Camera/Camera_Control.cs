using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using DG.Tweening;

public class Camera_Control : MonoBehaviour {

    public Camera Camera;
    public Camera Home_Camera;
    public Camera Game_Camera;
    public Camera Arrow_Camera;

    private Camera Target_Camera;

    private void Awake() {
        // DOTween.Init();
    }

    public void Set_Home_Camera() {
        // Animate_Camera(Home_Camera);
        Target_Camera = Home_Camera;
    }

    public void Set_Game_Camera() {
        // Animate_Camera(Game_Camera);
        Target_Camera = Game_Camera;
    }

    public void Set_Bow_Cam() {
        Target_Camera = GDG.Player_Control.Bow_Camera;
        Arrow_Camera.GetComponent<Arrow_Follow_Cam>().Deactivate();
    }

    public void Set_Arrow_Cam() {
        Target_Camera = Arrow_Camera;
        Arrow_Camera.GetComponent<Arrow_Follow_Cam>().Activate();
    }

    // Animation ---------------------------------
    private void Animate_Camera(Camera target, float duration = 0.5f) {
        // Sequence anim_seq = DOTween.Sequence();
        // anim_seq.Insert(0, Camera.DOFieldOfView(target.fieldOfView, duration) );
        // anim_seq.Insert(0, Camera.transform.DOMove(target.transform.position, duration) );
        // anim_seq.Insert(0, Camera.transform.DORotate(target.transform.eulerAngles, duration) );
    }
    // -------------------------------------------

    private void LateUpdate() {
        if(Target_Camera != null) {
            Camera.transform.position = Target_Camera.transform.position;
            Camera.transform.rotation = Target_Camera.transform.rotation;
            Camera.fieldOfView = Target_Camera.fieldOfView;
        }
    }

}
