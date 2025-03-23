using System.Collections;
using System.Collections.Generic;
using Game.Script.Inventory;
using UnityEngine;

public class Bow_Control : MonoBehaviour
{
    public Animator Bow_Animator;
    public float Shoot_Velocity = 10f;
    private float Arrow_Control_Sensitivity = 0.25f;

    private Trajectory_Drawer _Trajectory_Drawer;

    public Trajectory_Drawer Trajectory_Drawer
    {
        get
        {
            if (_Trajectory_Drawer == null)
            {
                _Trajectory_Drawer = GetComponent<Trajectory_Drawer>();
            }

            return _Trajectory_Drawer;
        }
    }

    public List<Arrow> Active_Arrows = new List<Arrow>();

    private e_Bow_State State = e_Bow_State.Idle;

    private float Fixed_Delta_Time;
    private float Slow_Motion_Ratio = 0.2f;
    // private float Slower_Motion_Ratio = 0.1f;

    private IEnumerator Toggle_Delay_Speed_Up;

    private Vector3 Last_Arrows_Center;
    private InventoryManager _inventoryManager;

    public int MaxActiveArrows => _inventoryManager.EquippedBow.maxActiveArrows;

    private void Awake()
    {
        _inventoryManager = InventoryManager.Instance;

        Fixed_Delta_Time = Time.fixedDeltaTime;
    }

    public void On_Pointer_Down(Vector2 pos)
    {
        switch (State)
        {
            case e_Bow_State.Idle:
                Trajectory_Drawer.Show_Trajectory();
                Trajectory_Drawer.Update_Trajectory();
                Set_State(e_Bow_State.Aiming);
                Bow_Animator.Play("Aim", -1, 0f);
                GDG.Audio_Control.Play_Arrow_Pull();
                GDG.UI_Control.Tutorial_Elements.Hide_Aim_Tutorial();
                break;
            case e_Bow_State.Arrow_Control:
                // Toggle_Set_Time_Scale_Slow();
                GDG.UI_Control.Tutorial_Elements.Hide_Control_Tutorial();
                break;
        }
    }

    public void On_Drag(Vector2 delta)
    {
        switch (State)
        {
            case e_Bow_State.Aiming:
                GDG.Player_Control.Aim_Control.On_Drag(delta);
                Trajectory_Drawer.Update_Trajectory();
                break;
            case e_Bow_State.Arrow_Control:
                Rotate_Arrows(delta);
                break;
        }
    }

    public void On_Pointer_Up(Vector2 pos)
    {
        switch (State)
        {
            case e_Bow_State.Aiming:
                Fire();
                Trajectory_Drawer.Hide_Trajectory();
                GDG.Camera_Control.Set_Arrow_Cam();
                Set_State(e_Bow_State.Arrow_Control);
                StartCoroutine(Delay_Set_Time_Scale_Slow());
                GDG.Audio_Control.Play_Arrow_Shot();
                GDG.UI_Control.Tutorial_Elements.Show_Control_Tutorial();
                break;
            case e_Bow_State.Arrow_Control:
                // Toggle_Set_Time_Scale_Normal();
                break;
        }
    }

    // ------------------------


    public void Fire()
    {
        GameObject arrow_obj = Instantiate(_inventoryManager.EquippedArrow.playerArrowPrefab, transform.position,
            transform.rotation, GDG.Level_Control.transform);
        Arrow arrow = arrow_obj.GetComponent<Arrow>();

        Vector3 arrow_velocity = transform.forward * Shoot_Velocity;
        arrow.Init(arrow_velocity);
        Bow_Animator.Play("Shoot", -1, 0f);
    }

    // ---------------------------

    public void Register_Active_Arrow(Arrow arrow)
    {
        if (!Active_Arrows.Contains(arrow))
        {
            Active_Arrows.Add(arrow);
        }
    }

    public void Unregister_Active_Arrow(Arrow arrow)
    {
        Active_Arrows.Remove(arrow);
        if (Active_Arrows.Count == 0)
        {
            GDG.Player_Control.Aim_Control.Aim_To_Target(Last_Arrows_Center);
            GDG.MGC.Call_Delay(() =>
            {
                if (Active_Arrows.Count == 0)
                {
                    GDG.Camera_Control.Set_Bow_Cam();
                    Set_State(e_Bow_State.Idle);
                }
            }, 0.2f);
        }
    }

    // --------------

    public bool Has_Active_Arrows()
    {
        return Active_Arrows.Count > 0;
    }

    public Vector3 Get_Active_Arrows_Center()
    {
        Vector3 center = new Vector3();
        for (int i = 0; i < Active_Arrows.Count; i++)
        {
            center += Active_Arrows[i].transform.position;
        }

        center = center / Active_Arrows.Count;
        Last_Arrows_Center = center;
        return center;
    }

    public Vector3 Get_Active_Arrows_Direction()
    {
        Vector3 direction = new Vector3();
        if (Active_Arrows.Count > 0)
        {
            direction = Active_Arrows[0].transform.forward;
        }

        return direction;
    }

    private float Arrow_Distance_In_Bundle = 0.5f;
    private float Radius_Step_In_Bundle = 0.6f;

    public void Arrange_Arrows()
    {
        // Arrange in circular shape

        Transform center_arrow = Active_Arrows[0].transform;
        float radius = Radius_Step_In_Bundle;
        int index_in_circle = 0;
        for (int i = 1; i < Active_Arrows.Count; i++)
        {
            Arrow arrow = Active_Arrows[i];
            float perimeter = 2f * Mathf.PI * radius;
            float tetha = (Arrow_Distance_In_Bundle / perimeter) * 360f;
            float angle = tetha * (float)index_in_circle;
            if (angle > 360f - tetha)
            {
                index_in_circle = 0;
                radius += Radius_Step_In_Bundle;
                angle = 0f;
            }

            arrow.transform.position =
                center_arrow.position + Get_Arrow_Position_In_Circle(center_arrow, radius, angle);
            index_in_circle++;
        }
    }

    private Vector3 Get_Arrow_Position_In_Circle(Transform center_arrow, float radius, float angle)
    {
        Vector3 pos = new Vector3();
        pos = Quaternion.AngleAxis(angle, center_arrow.forward) * center_arrow.right * radius;
        return pos;
    }


    // --------------------------------------

    private void Rotate_Arrows(Vector2 delta)
    {
        Vector3 center = Get_Active_Arrows_Center();
        for (int i = 0; i < Active_Arrows.Count; i++)
        {
            float rot_x = delta.x * Arrow_Control_Sensitivity;
            float rot_y = -delta.y * Arrow_Control_Sensitivity;
            // Velocity
            Vector3 vel = Active_Arrows[i].Get_Rigid_Body().velocity;
            vel = Quaternion.AngleAxis(rot_x, Active_Arrows[i].transform.up) * vel;
            vel = Quaternion.AngleAxis(rot_y, Active_Arrows[i].transform.right) * vel;
            Active_Arrows[i].Get_Rigid_Body().velocity = vel;
            // Position
            Vector3 pos = Active_Arrows[i].Get_Rigid_Body().position;
            pos = pos.Rotate_Around_Pivot(center, Active_Arrows[i].transform.up * rot_x);
            pos = pos.Rotate_Around_Pivot(center, Active_Arrows[i].transform.right * rot_y);
            Active_Arrows[i].Get_Rigid_Body().position = pos;
        }
    }

    // --------------------------------------

    private void Set_State(e_Bow_State state)
    {
        State = state;
        switch (State)
        {
            case e_Bow_State.Idle:
            case e_Bow_State.Aiming:
                Set_Time_Scale(1f);
                break;
        }
    }

    private void Set_Time_Scale(float scale)
    {
        Time.timeScale = scale;
    }

    private IEnumerator Delay_Set_Time_Scale_Slow()
    {
        yield return new WaitForSecondsRealtime(0.35f);
        Set_Time_Scale_Slow();
    }

    private void Set_Time_Scale_Normal()
    {
        Set_Time_Scale(1f);
    }

    private void Set_Time_Scale_Slow()
    {
        Set_Time_Scale(Slow_Motion_Ratio);
    }

    // private void Toggle_Set_Time_Scale_Slow() {
    //     Set_Time_Scale_Slow();
    //     if(Toggle_Delay_Speed_Up != null) {
    //         StopCoroutine(Toggle_Delay_Speed_Up);
    //     }
    // }

    // private void Toggle_Set_Time_Scale_Normal() {
    //     Toggle_Delay_Speed_Up = Delay_Speed_Up();
    //     StartCoroutine(Toggle_Delay_Speed_Up);
    // }

    // private IEnumerator Delay_Speed_Up() {
    //     yield return new WaitForSecondsRealtime(1f);
    //     Set_Time_Scale_Normal();
    // }

    // private void Set_Time_Scale_Slower() {
    //     Set_Time_Scale(Slower_Motion_Ratio);
    // }


    // -------------------------------------

    private void Update()
    {
        // if(Input.GetKey("s")) { Set_Time_Scale_Slow(); } else { Set_Time_Scale_Normal(); }
        Time.fixedDeltaTime = Fixed_Delta_Time * Time.timeScale;
    }

    // -------------------------------------

    public void Stop_All()
    {
        // for(int i = 0; i < Active_Arrows.Count; i++) {
        //     Destroy(Active_Arrows[i].gameObject);
        // }
        Set_Time_Scale_Normal();
    }

    public enum e_Bow_State
    {
        Idle,
        Aiming,
        Arrow_Control
    }
}