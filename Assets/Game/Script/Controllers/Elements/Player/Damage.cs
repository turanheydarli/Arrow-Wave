using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Damage {

    public e_Damage_Type Damage_Type;
    public float Intensity;
    public Object Source;
    public e_Entity_Side Attacker_Side;

    public Damage(e_Damage_Type damage_Type, float intensity, Object source, e_Entity_Side attacker_side) {
        Damage_Type = damage_Type;
        Intensity = intensity;
        Source = source;
        Attacker_Side = attacker_side;
    }

    // --------------------------------------
    public enum e_Damage_Type {
        Slash,
        Pierce,
        Explosion,
    }
}

public enum e_Entity_Side {
    None,
    Player,
    Enemy
}

