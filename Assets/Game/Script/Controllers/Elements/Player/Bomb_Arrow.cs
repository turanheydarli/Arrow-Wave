using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb_Arrow : Arrow {

    [Header("Bomb Arrow")]
    public float Explosion_Damage = 200f;
    public float Explosion_Range = 6f;
    public float Explosion_Force = 1f;
    public GameObject Explosion_Prefab;

    protected override void On_Hit(Collider other) {
        base.On_Hit(other);
        Multiplier mult = other.gameObject.GetComponent<Multiplier>();
        if(mult == null) {
            Explode();
        }
    }

    protected override void Destroy_On_Hit() {
        Destroy_Self();
    }

    private void Explode() {
        GameObject explosion = Instantiate(Explosion_Prefab, transform.position, transform.rotation, GDG.Level_Control.transform);
        Explosion exp = explosion.GetComponent<Explosion>();
        exp.Explode(Explosion_Range, Explosion_Damage, Explosion_Force, Side);
    }

}
