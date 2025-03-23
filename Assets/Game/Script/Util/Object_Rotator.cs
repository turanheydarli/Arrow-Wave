using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Object_Rotator : MonoBehaviour {
    public Vector3 Rotation;

    private void FixedUpdate() {
        float dt = Time.fixedDeltaTime;
        transform.localEulerAngles += Rotation * dt;
    }

}
