using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_Enemy_Control : Enemy_Control {
    
    private void Start() {
        Attack_Damage_Delay = 1.55f;
        Movement_Control.Set_Movement_Speed(1.3f);
        Attack_Range = 2.5f;
        Attack_Interval = 3f;
    }
    
}
