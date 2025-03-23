using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Control : MonoBehaviour {

    private Animator Character_Animator;
    private List<Rigidbody> Rigid_Bodies = new List<Rigidbody>();
    private List<Collider> Colliders = new List<Collider>();

    private void Awake() {
        Character_Animator = GetComponentInChildren<Animator>();
        Prepare_Ragdoll_Elements();
        Deactivate_Ragdoll();
    }

    // Animation --------------------------

    public void Set_Movement_Vector(Vector3 vector) {

        if(vector != Vector3.zero) {
            Play_Walk_Animation();
        } else {
            Play_Idle_Animation();
        }
    }

    private void Play_Idle_Animation() {
        if(Character_Animator.GetBool("Walk") == true) {
            Character_Animator.SetBool("Walk", false);
        }
    }

    private void Play_Walk_Animation() {
        if(Character_Animator.GetBool("Walk") == false) {
            Character_Animator.SetBool("Walk", true);
        }
    }

    public void Play_Attack_Animation() {
        Character_Animator.SetTrigger("Attack");
    }

    // Ragdoll ----------------------------
    
    private void Prepare_Ragdoll_Elements() {
        Rigidbody[] rb_list = Character_Animator.GetComponentsInChildren<Rigidbody>();
        for(int i = 0; i < rb_list.Length; i++) {
            Rigid_Bodies.Add(rb_list[i]);
        }
        Collider[] col_list = Character_Animator.GetComponentsInChildren<Collider>();
        for(int i = 0; i < col_list.Length; i++) {
            Colliders.Add(col_list[i]);
        }
    }

    public void Activate_Ragdoll() {
        Character_Animator.enabled = false;
        for(int i = 0; i < Rigid_Bodies.Count; i++) {
            Rigid_Bodies[i].isKinematic = false;
        }
        for(int i = 0; i < Colliders.Count; i++) {
            // Colliders[i].enabled = true;
            Colliders[i].isTrigger = false;
        }
    }

    public void Deactivate_Ragdoll() {
        Character_Animator.enabled = true;
        for(int i = 0; i < Rigid_Bodies.Count; i++) {
            Rigid_Bodies[i].isKinematic = true;
        }
        for(int i = 0; i < Colliders.Count; i++) {
            // Colliders[i].enabled = false;
            Colliders[i].isTrigger = true;
        }
    }

}
