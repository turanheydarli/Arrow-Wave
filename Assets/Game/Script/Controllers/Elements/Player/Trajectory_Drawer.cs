using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Bow_Control))]
public class Trajectory_Drawer : MonoBehaviour {

    private Bow_Control _Bow_Control;
    public Bow_Control Bow_Control {
        get {
            if(_Bow_Control == null) {
                _Bow_Control = GetComponent<Bow_Control>();
            }
            return _Bow_Control;
        }
    }

    public Transform Trajectory_Group;

    public GameObject Trajectory_Strip_Prefab;
    public GameObject Crosshair_Prefab;


    private int Max_Points = 200;
    private float Timestep = 0.02f;

    private float Strip_Start_Scale = 0.35f;
    private float Strip_End_Scale = 4f;
    private float Strip_Step_Scale = 0.15f;

    private List<Vector3> Points = new List<Vector3>();
    private Vector3 Hit_Position = new Vector3();

    public LayerMask Ignore_Layers;

    public void Show_Trajectory() {
        Trajectory_Group.gameObject.SetActive(true);
    }

    public void Hide_Trajectory() {
        Trajectory_Group.gameObject.SetActive(false);
    }

    // ---------------------------------

    public void Update_Trajectory() {
        Update_Trajectory_Line();
        Update_Crosshair();
    }

    // ---------------------------------

    private void Update_Trajectory_Line() {
        Clear_Trajectory_Line();
        Generate_Trajectory_Points();
        Generate_Trajectory_Line();
    }

    private void Clear_Trajectory_Line() {
        for(int i = 0; i < Trajectory_Group.transform.childCount; i++) {
            Destroy(Trajectory_Group.transform.GetChild(i).gameObject);
        }
    }

    private void Generate_Trajectory_Points() {
        // Calculate
        Vector3 shoot_velocity = transform.forward * Bow_Control.Shoot_Velocity;
        Vector3 gravity = Physics.gravity;
        Vector3 current_point = transform.position;
        Vector3 current_velocity = shoot_velocity;
        List<Vector3> pts = new List<Vector3>();
        pts.Clear();
        pts.Add(current_point);
        for(int i = 0; i < Max_Points; i++) {
            current_velocity += Timestep * gravity;
            current_point += Timestep * current_velocity;
            pts.Add(current_point); // i+1
            if(Test_Line(pts[i], pts[i + 1], ref Hit_Position)) {
                pts[i + 1] = Hit_Position;
                break;
            }
        }

        // Filter
        Points.Clear();
        for(int i = 0; i < pts.Count; i += 4) {
            Points.Add(pts[i]);
        }
        if(Points[Points.Count - 1] != pts[pts.Count - 1]) {
            Points.Add(pts[pts.Count - 1]);
        }
    }

    private bool Test_Line(Vector3 p1, Vector3 p2, ref Vector3 hit_pos) {
        RaycastHit hit;
        Vector3 dir = p2 - p1;

        if(Physics.Raycast(p1, dir, out hit, dir.magnitude, ~Ignore_Layers)) {
            hit_pos = hit.point;
            return true;
        } else {
            return false;
        }

    }

    private void Generate_Trajectory_Line() {
        float strip_scale = Strip_Start_Scale;
        for(int i = 0; i < Points.Count - 1; i++) {
            Vector3 start = Points[i];
            Vector3 end = Points[i + 1];
            Vector3 dir = end - start;
            Quaternion rot = Quaternion.LookRotation(dir);
            GameObject strip = Instantiate(Trajectory_Strip_Prefab, start, rot, Trajectory_Group);
            Vector3 scale = strip.transform.localScale;
            scale.x = scale.y = Mathf.Min(strip_scale + Strip_Step_Scale * i, Strip_End_Scale);
            scale.z = dir.magnitude * (i < Points.Count - 2 ? 0.5f : 1.0f);
            strip.transform.localScale = scale;
        }
    }

    // ---------------------------------

    private void Update_Crosshair() {
        GameObject crosshair = Instantiate(Crosshair_Prefab, Hit_Position, Quaternion.identity, Trajectory_Group);
        crosshair.transform.rotation = Quaternion.LookRotation(transform.position - Hit_Position, Vector3.up);
    }



    // private void OnDrawGizmos() {
    //     Update_Trajectory();
    //     Gizmos.color = Color.yellow;
    //     for(int i = 0; i < Points.Count; i++) {
    //         Gizmos.DrawSphere(Points[i], 0.05f);
    //     }
    // }

}
