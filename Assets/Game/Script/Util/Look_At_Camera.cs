using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Look_At_Camera : MonoBehaviour {

    private void LateUpdate() {
        transform.LookAt(transform.position + GDG.Camera_Control.Camera.transform.forward);
    }
}
