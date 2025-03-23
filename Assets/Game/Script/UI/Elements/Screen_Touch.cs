using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class Screen_Touch : MonoBehaviour , IDragHandler, IPointerDownHandler, IPointerUpHandler
{
    public float Height {get; private set;}
    public float Inv_Height {get; private set;}


    private void Awake() {
        Height = GetComponent<RectTransform>().rect.height;
        Inv_Height = 1f/Height;
    }

    public void OnPointerDown(PointerEventData data){
        GDG.Input_Control.On_Pointer_Down(data.position);
    }

    public void OnPointerUp(PointerEventData data){
        GDG.Input_Control.On_Pointer_Up(data.position);
    }

    public void OnDrag(PointerEventData data){
        GDG.Input_Control.On_Drag(data.delta);
    }

}
