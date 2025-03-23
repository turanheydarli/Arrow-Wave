using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class Tap_Button : MonoBehaviour, IPointerDownHandler
{

    public UnityEvent Button_Event;

    public void OnPointerDown(PointerEventData eventData) {
        Button_Event.Invoke();
    }

}
