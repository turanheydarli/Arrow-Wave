using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Mover : MonoBehaviour {
    
    private Vector3 Base_Pos;
    public float Animate_Speed = 3f;
    public Vector3 Animate_Direction;

    // Start is called before the first frame update
    void Start() {
        Base_Pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Vector3 offset = Mathf.Sin(Time.fixedTime * Animate_Speed) * Animate_Direction;
        transform.position = Base_Pos + offset;
    }
}
