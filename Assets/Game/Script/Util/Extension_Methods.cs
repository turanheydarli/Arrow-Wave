using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/* USAGE:
var myGameObject = transform.FindObjectsWithTag("myTag").FirstOrDefault();
if (myGameObject != null){
    // ...
} 
*/

public static class GDG_Utils{
    
    public static List<GameObject> FindObjectsWithTag(this Transform parent, string tag){
        List<GameObject> taggedGameObjects = new List<GameObject>();

        for (int i = 0; i < parent.childCount; i++){
            Transform child = parent.GetChild(i);
            if (child.tag == tag){
                taggedGameObjects.Add(child.gameObject);
            }
            if (child.childCount > 0){
                taggedGameObjects.AddRange(FindObjectsWithTag(child, tag));
            }
        }
        return taggedGameObjects;
    }

    public static Vector2 Rotate(this Vector2 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

    public static Vector3 Rotate_Z(this Vector3 v, float delta) {
        delta *= Mathf.Deg2Rad;
        return new Vector3(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta),
            v.z
        );
    }

    public static Vector3 Rotate_Around_Pivot(this Vector3 point, Vector3 pivot, Vector3 angles) {
        return Quaternion.Euler(angles) * (point - pivot) + pivot;
    }

}


