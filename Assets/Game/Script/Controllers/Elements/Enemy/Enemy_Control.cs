using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Control : Damageable {
    protected Movement_Control Movement_Control;
    protected Character_Control Character_Control;

    private e_Enemy_State State;
    private float State_Update_Interval = 0.1f;

    private float Max_Health;
    private Health_Bar Health_Bar;

    // Idle
    private float Idle_Time_Default = 1f;
    private float Idle_Time = 1f;

    // Walk
    public List<Vector3> Waypoints = new List<Vector3>();
    public bool Loop_Waypoints = false;
    private int Current_Waypoint_Index = 0;
    private Vector3 Current_Waypoint { get { if(Waypoints.Count > Current_Waypoint_Index) return Waypoints[Current_Waypoint_Index]; else return Vector3.zero; } }
    private float Reach_Distance = 0.25f;

    // Attack Target
    [Min(0f)]
    public float Hit_Damage = 50f;
    private Damageable Target_To_Attack;
    protected float Attack_Range = 2f;
    protected float Attack_Interval = 1.5f;
    private float Attack_Countdown = 1.5f;

    protected float Attack_Damage_Delay = 0.5f;
    private IEnumerator Delay_Apply_Damage_Coroutine;
    private bool Delay_Apply_Damage_Running = false;

    private Rigidbody Main_Rigid_Body;
    private Collider Main_Collider;

    public GameObject Blood_Particle;


    // -------------------------------------------

    private void Awake() {
        Movement_Control = GetComponent<Movement_Control>();
        Character_Control = GetComponent<Character_Control>();
        Main_Rigid_Body = GetComponent<Rigidbody>();
        Main_Collider = GetComponent<Collider>();
        GDG.Game_Control.Add_Slow_Update_Listener(Slow_Update);
        GDG.Game_Control.Add_Fixed_Update_Listener(Fixed_Update_Enemy_Control);
        Health_Bar = GetComponentInChildren<Health_Bar>();
        Init_Enemy();
    }

    private void OnDestroy() {
        GDG.Game_Control.Remove_Slow_Update_Listener(Slow_Update);
        GDG.Game_Control.Remove_Fixed_Update_Listener(Fixed_Update_Enemy_Control);
    }

    // -------------------------------------------

    public void Init_Enemy() {
        Max_Health = Health;
        Update_Health_Bar();
        Set_State(e_Enemy_State.Idle);
    }

    // -------------------------------------------

    private void Update_Health_Bar() {
        if(Health_Bar == null) return;
        Health_Bar.Set_Progress(Health / Max_Health);
        if(Health > 0) {
            if(!Health_Bar.Is_Active()) {
                Health_Bar.Show();
            }
        } else {
            if(Health_Bar.Is_Active()) {
                Health_Bar.Hide();
            }
        }
    }

    // -------------------------------------------

    private void Set_State(e_Enemy_State state) {
        switch(state) {
            case e_Enemy_State.Idle:
                Set_Idle();
                break;
            case e_Enemy_State.Walk_To_Waypoint:
                Set_Walk_To_Waypoint();
                break;
            case e_Enemy_State.Walk_To_Target:
                Set_Walk_To_Target();
                break;
            case e_Enemy_State.Attack_Target:
                Set_Attack_Target();
                break;
            case e_Enemy_State.Dizzy:
                break;
            case e_Enemy_State.Dead:
                Set_Dead();
                break;
        }
    }

    // -------------------------------------------

    private void Set_Idle() {
        Movement_Control.Set_Movement_Vector(Vector2.zero);
        Idle_Time = Idle_Time_Default;
        State = e_Enemy_State.Idle;
    }

    private void Set_Walk_To_Waypoint() {
        State = e_Enemy_State.Walk_To_Waypoint;
    }

    private void Set_Walk_To_Target() {
        Damageable target = Find_Closest_Target();
        Target_To_Attack = target;
        State = e_Enemy_State.Walk_To_Target;
    }

    private void Set_Attack_Target() {
        Reset_Movement_Vector();
        State = e_Enemy_State.Attack_Target;
    }

    private void Set_Dead() {
        Reset_Movement_Vector();
        State = e_Enemy_State.Dead;
    }

    // -------------------------------------------

    private Damageable Find_Closest_Target() {
        Damageable[] targets = GDG.Level_Control.Level_Elements.GetComponentsInChildren<Damageable>();
        List<Damageable> player_targets = new List<Damageable>();

        for(int i = 0; i < targets.Length; i++) { // Filter player entities
            if(targets[i].Entity_Side == e_Entity_Side.Player && targets[i].Is_Alive()) {
                player_targets.Add(targets[i]);
            }
        }

        if(player_targets.Count > 0) {
            player_targets.Sort(Compare_Targets);
            return player_targets[0];
        } else {
            return null;
        }
    }

    private int Compare_Targets(Damageable t1, Damageable t2) {
        float d1 = (transform.position - t1.transform.position).sqrMagnitude;
        float d2 = (transform.position - t2.transform.position).sqrMagnitude;
        if(d1 > d2) {
            return 1;
        } else if(d2 > d1) {
            return -1;
        } else {
            return 0;
        }
    }


    // -------------------------------------------

    private void Fixed_Update_Enemy_Control() {
        if(State != e_Enemy_State.Dizzy && State != e_Enemy_State.Dead) {
            Movement_Control.Fixed_Update_Movement_Control();
        }
    }

    // -------------------------------------------


    private void Slow_Update() {
        Update_State(State_Update_Interval);
    }

    private void Update_State(float delta_time) {
        switch(State) {
            case e_Enemy_State.Idle:
                Update_Idle(delta_time);
                break;
            case e_Enemy_State.Walk_To_Waypoint:
                Update_Walk_To_Waypoint(delta_time);
                break;
            case e_Enemy_State.Walk_To_Target:
                Update_Walk_To_Target(delta_time);
                break;
            case e_Enemy_State.Attack_Target:
                Update_Attack_Target(delta_time);
                break;
            case e_Enemy_State.Dizzy:
                break;
            case e_Enemy_State.Dead:
                break;
        }
    }

    // Idle -----------------

    private void Update_Idle(float delta_time) {
        Idle_Time -= delta_time;
        if(Idle_Time < 0) {
            Set_State(e_Enemy_State.Walk_To_Waypoint);
        }
    }

    // Walk to Waypoint -----

    private void Update_Walk_To_Waypoint(float delta_time) {
        Set_Movement_Vector_For_Waypoint(Current_Waypoint);
        if(Waypoint_Reached(Current_Waypoint)) {
            Set_Next_Waypoint();
        }
    }

    private void Update_Walk_To_Target(float delta_time) {
        if(Target_To_Attack == null) { return; }
        Set_Movement_Vector_For_Waypoint(Target_To_Attack.transform.position);
        if(Target_In_Range(Target_To_Attack.transform.position)) {
            Set_State(e_Enemy_State.Attack_Target);
        }
    }

    private void Set_Movement_Vector_For_Waypoint(Vector3 waypoint) {
        Vector2 current_pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 waypoint_pos = new Vector2(waypoint.x, waypoint.z);
        Movement_Control.Set_Movement_Vector((waypoint_pos - current_pos).normalized);
    }

    private void Reset_Movement_Vector() {
        Movement_Control.Set_Movement_Vector(Vector2.zero);
    }

    private void Set_Look_Vector_For_Waypoint(Vector3 waypoint) {
        Vector2 current_pos = new Vector2(transform.position.x, transform.position.z);
        Vector2 waypoint_pos = new Vector2(waypoint.x, waypoint.z);
        Movement_Control.Set_Look_Vector((waypoint_pos - current_pos).normalized);
    }

    private bool Waypoint_Reached(Vector3 waypoint) {
        Vector3 current_pos = transform.position;
        Vector3 current_wp = waypoint;
        current_pos.y = 0f;
        current_wp.y = 0f;
        Vector3 dist = current_wp - current_pos;
        if(dist.sqrMagnitude < Reach_Distance * Reach_Distance) {
            return true;
        } else {
            return false;
        }
    }

    private void Set_Next_Waypoint() {
        Current_Waypoint_Index++;
        if(Current_Waypoint_Index > Waypoints.Count - 1) {
            Reached_Last_Waypoint();
        }
    }

    private void Reached_Last_Waypoint() {
        if(Loop_Waypoints) {
            Current_Waypoint_Index = 0;
        } else {
            Set_State(e_Enemy_State.Walk_To_Target);
        }
    }

    // Attack Target --------

    private void Update_Attack_Target(float delta_time) {
        if(Target_To_Attack == null) { return; }
        if(Target_To_Attack.Is_Alive() && Target_In_Range(Target_To_Attack.transform.position)) {
            Set_Look_Vector_For_Waypoint(Target_To_Attack.transform.position);
            Countdown_And_Attack(delta_time);
        } else {
            Set_State(e_Enemy_State.Walk_To_Target);
        }
    }

    private void Countdown_And_Attack(float delta_time) {
        Attack_Countdown -= delta_time;
        if(Attack_Countdown <= 0f) {
            Attack_Countdown = Attack_Interval;
            Trigger_Attack();
        }
    }

    private void Trigger_Attack() {
        // Play attack animation
        Character_Control.Play_Attack_Animation();
        // Apply Damage on hit
        Delay_Apply_Damage_Coroutine = Delay_Apply_Damage();
        StartCoroutine(Delay_Apply_Damage_Coroutine);
    }

    private IEnumerator Delay_Apply_Damage() {
        Delay_Apply_Damage_Running = true;
        yield return new WaitForSeconds(Attack_Damage_Delay);
        Target_To_Attack.Apply_Damage(new Damage(Damage.e_Damage_Type.Slash, Hit_Damage, this, e_Entity_Side.Enemy));
        GDG.Audio_Control.Play_Enemy_Attack();
        Delay_Apply_Damage_Running = false;
    }

    private bool Target_In_Range(Vector3 target) {
        Vector3 pos = transform.position;
        pos.y = 0f;
        target.y = 0f;
        Vector3 dist = target - pos;
        if(dist.sqrMagnitude < Attack_Range * Attack_Range) {
            return true;
        } else {
            return false;
        }
    }


    // -------------------------------------------

    public override void Apply_Damage(Damage damage) {
        base.Apply_Damage(damage);
        if(damage.Damage_Type == Damage.e_Damage_Type.Pierce) {
            Arrow hit_obj = (Arrow)damage.Source;
            if(hit_obj) {
                Vector3 blood_pos = hit_obj.transform.position;
                Quaternion blood_rot = hit_obj.transform.rotation;
                GameObject bp = Instantiate(Blood_Particle, blood_pos, blood_rot, GDG.Level_Control.Level_Elements);
            }
            GDG.Audio_Control.Play_Arrow_Impact();
        }
        Update_Health_Bar();
    }

    protected override void Die() {

        Character_Control.Activate_Ragdoll();

        Main_Rigid_Body.isKinematic = true;
        Main_Collider.enabled = false;

        if(Delay_Apply_Damage_Running) {
            StopCoroutine(Delay_Apply_Damage_Coroutine);
        }
        StartCoroutine(Delay_Destroy());
        Set_State(e_Enemy_State.Dead);
    }

    private IEnumerator Delay_Destroy() {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }

    // -------------------------------------------

    private void OnDrawGizmosSelected() {
        for(int i = 0; i < Waypoints.Count; i++) {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(Waypoints[i], 0.15f);
            if(i > 0) {
                Gizmos.DrawLine(Waypoints[i - 1], Waypoints[i]);
            }
        }
    }

    // -------------------------------------------

    public enum e_Enemy_State {
        Idle,
        Walk_To_Waypoint,
        Walk_To_Target,
        Attack_Target,
        Dizzy,
        Dead
    }
}
