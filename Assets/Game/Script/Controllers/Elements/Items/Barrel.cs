using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : Damageable {

    protected override void Die() {
        GDG.MGC.Call_Delay(() => { Destroy(gameObject); }, 1f);
    }
    
}
