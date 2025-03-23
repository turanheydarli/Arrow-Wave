using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Enemy_Control), true)]
public class Enemy_Control_Editor : Editor {
    protected virtual void OnSceneGUI() {
        Enemy_Control enemy = (Enemy_Control)target;

        List<Vector3> new_positions = new List<Vector3>(enemy.Waypoints);

        EditorGUI.BeginChangeCheck();
        for(int i = 0; i < new_positions.Count; i++) {
            new_positions[i] = Handles.PositionHandle(enemy.Waypoints[i], Quaternion.identity);
        }
        if(EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(enemy, "Change Waypoint");
            for(int i = 0; i < new_positions.Count; i++) {
                enemy.Waypoints[i] = new_positions[i];
            }
        }
    }
}

