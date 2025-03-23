using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/* 
GDG: Event_Target Class
Usage:
    Register Event:
        Component.On_Event(enum_event_id, callback);

    Unregister Event:
        Component.Off_Event(enum_event_id, callback);

    Emit Event:
        Component.Emit_Event(enum_event_id);

    // Add events enum
    public enum Event_Type {
        ON_FADE_IN,
        ON_FADE_OUT
    };

*/


public struct Event_Payload{
    public int i1;
    public string s1;

    public Event_Payload(int int_1){
        i1 = int_1;
        s1 = "";
    }

    public Event_Payload(string string_1){
        i1 = 0;
        s1 = string_1;
    }

    public Event_Payload(int int_1, string string_1){
        i1 = int_1;
        s1 = string_1;
    }

}

public class Event_1p : UnityEvent<Event_Payload>{}

public class Event_Target : MonoBehaviour {

    private Dictionary <Enum, Event_1p> eventDictionary;
    private Dictionary <Enum, UnityEvent> eventDictionary_np;
    

    private void Init (){
        if (eventDictionary == null){
            eventDictionary = new Dictionary<Enum, Event_1p>();
        }
        if (eventDictionary_np == null){
            eventDictionary_np = new Dictionary<Enum, UnityEvent>();
        }
    }

    public void On_Event (Enum eventName, UnityAction<Event_Payload> listener){
        Init(); // Make sure initialized
        Event_1p thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent)){
            thisEvent.AddListener (listener);
        }else{
            thisEvent = new Event_1p ();
            thisEvent.AddListener (listener);
            eventDictionary.Add (eventName, thisEvent);
        }
    }

    public void On_Event (Enum eventName, UnityAction listener){
        Init(); // Make sure initialized
        UnityEvent thisEvent = null;
        if (eventDictionary_np.TryGetValue (eventName, out thisEvent)){
            thisEvent.AddListener (listener);
        }else{
            thisEvent = new UnityEvent ();
            thisEvent.AddListener (listener);
            eventDictionary_np.Add (eventName, thisEvent);
        }
    }

    public void Off_Event (Enum eventName, UnityAction<Event_Payload> listener){
        Init(); // Make sure initialized
        Event_1p thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent)){
            thisEvent.RemoveListener (listener);
        }
    }

    public void Off_Event (Enum eventName, UnityAction listener){
        Init(); // Make sure initialized
        UnityEvent thisEvent = null;
        if (eventDictionary_np.TryGetValue (eventName, out thisEvent)){
            thisEvent.RemoveListener (listener);
        }
    }

    public void Emit_Event (Enum eventName, Event_Payload param1){
        Init(); // Make sure initialized
        Event_1p thisEvent = null;
        if (eventDictionary.TryGetValue (eventName, out thisEvent)){
            thisEvent.Invoke(param1);
        }
    }

    public void Emit_Event (Enum eventName){
        Init(); // Make sure initialized
        UnityEvent thisEvent = null;
        if (eventDictionary_np.TryGetValue (eventName, out thisEvent)){
            thisEvent.Invoke();
        }
    }
  
}





