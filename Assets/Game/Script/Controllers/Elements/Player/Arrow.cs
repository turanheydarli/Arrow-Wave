using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Arrow : MonoBehaviour {

    [Header("Arrow")]
    public float Pierce_Damage = 50f;

    private Rigidbody Rigid_Body;
    private e_Arrow_State State = e_Arrow_State.On_Air;
    private Quaternion Restore_Rotation;
    public Vector3 Restore_Velocity { get; private set; }
    public e_Entity_Side Side;

    private void Awake() {
        Rigid_Body = GetComponent<Rigidbody>();
    }

    public void Init(Vector3 arrow_velocity) {
        Set_State(e_Arrow_State.On_Air);
        Rigid_Body.isKinematic = false;
        Rigid_Body.velocity = arrow_velocity;
        enabled = true;
    }

    protected virtual void FixedUpdate() {
        if(State == e_Arrow_State.On_Air) {
            Align_To_Velocity();
        }
    }

    private void OnCollisionEnter(Collision collision) {
        Debug.Log($"Arrow {GetInstanceID()} collided with {collision.collider.name}", this);
        On_Hit(collision.collider);
    }
    private void OnTriggerEnter(Collider other) {
        Debug.Log($"Arrow {GetInstanceID()} TriggerEnter with {other.name}", this);
        On_Hit(other);
    }


    // --------------------

    protected virtual void On_Hit(Collider other)
    {
        if (State != e_Arrow_State.On_Air) return;

        Damageable hit_obj = other.GetComponentInParent<Damageable>();
        if (hit_obj)
        {
            if (hit_obj is Multiplier)
            {
                hit_obj.Apply_Damage(new Damage(Damage.e_Damage_Type.Pierce, Pierce_Damage, this, Side));
            
                Unregister_Active_Arrow();
            
                Destroy_On_Multiplier_Hit();
            
                return; 
            }
            else
            {
                if (hit_obj.Can_Arrow_Stab) Stab_Arrow(hit_obj, other);
                else Drop_Arrow();

                hit_obj.Apply_Damage(new Damage(Damage.e_Damage_Type.Pierce, Pierce_Damage, this, Side));
            }
        }
        else
        {
            Drop_Arrow();
        }
    
        GDG.Audio_Control.Play_Haptic_Medium();
        GDG.Audio_Control.Play_Arrow_Hit();
    }



    // --------------------

    private void Drop_Arrow() {
        Set_State(e_Arrow_State.Dropped);
    }

    private void Stab_Arrow(Damageable hit_obj, Collider collider) {
        Rigidbody rb = collider.GetComponentInParent<Rigidbody>();
        if(rb != null) {
            // Create joint
            transform.rotation = Restore_Rotation;
            Joint jnt = gameObject.AddComponent<FixedJoint>();
            jnt.connectedBody = rb;
            transform.parent = rb.transform.parent;
        } else {
            Rigid_Body.isKinematic = true;
            transform.rotation = Restore_Rotation;
        }
        hit_obj.Add_Destroy_Listener(Destroy_Self);
        Set_State(e_Arrow_State.Stabbed);
    }

    // --------------------

    private void Set_State(e_Arrow_State newState) {
        Debug.Log($"Arrow {GetInstanceID()} transitioning from {State} to {newState}", this);
        State = newState;
        switch(newState) {
            case e_Arrow_State.On_Air:
                Register_Active_Arrow();
                break;
            case e_Arrow_State.Stabbed:
            case e_Arrow_State.Dropped:
                Unregister_Active_Arrow();
                Destroy_On_Hit();
                break;
        }
    }


    // --------------------

    protected virtual void Destroy_On_Hit() {
        GDG.MGC.Call_Delay(Destroy_Self, 5f);
    }

    protected void Destroy_Self() {
        if(this != null && gameObject != null) {
            Destroy(gameObject);
        }
    }

    public void Destroy_On_Multiplier_Hit() {
        Destroy(gameObject);
    }

    // -------------------------------

    private void Align_To_Velocity() {
        transform.rotation = Quaternion.LookRotation(Rigid_Body.velocity);
        Restore_Rotation = transform.rotation;
        Restore_Velocity = Rigid_Body.velocity;
    }

    private void Register_Active_Arrow() {
        GDG.Player_Control.Bow_Control.Register_Active_Arrow(this);
    }

    private void Unregister_Active_Arrow() {
        GDG.Player_Control.Bow_Control.Unregister_Active_Arrow(this);
    }

    // ------------------------------

    public Rigidbody Get_Rigid_Body() {
        return Rigid_Body;
    }


    public enum e_Arrow_State {
        On_Air,
        Stabbed,
        Dropped
    }
}
