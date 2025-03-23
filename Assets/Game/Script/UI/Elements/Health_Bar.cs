using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
public class Health_Bar : MonoBehaviour {

    public Image Health_Fill;

    public void Show() {
        gameObject.SetActive(true);
    }

    public void Hide() {
        gameObject.SetActive(false);
    }

    public bool Is_Active() {
        return gameObject.activeSelf;
    }

    public void Set_Progress(float ratio) {
        Health_Fill.fillAmount = ratio;
    }
    
    // -------------------------------------
    
    private void Start() {
        Canvas can = GetComponent<Canvas>();
        can.worldCamera = GDG.Camera_Control.Camera;
    }

    private void LateUpdate() {
        transform.LookAt(transform.position + GDG.Camera_Control.Camera.transform.forward);
    }

}
