using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MyBox;

public class Explosion : MonoBehaviour {

    // public Transform Explosion_Sphere;
    public ParticleSystem Explode_Paticles;

    private LayerMask Arrows_Mask;

    public bool Explode_On_Awake = false;
    [ConditionalField(nameof(Explode_On_Awake))]
    public float Explode_Range = 6f;
    [ConditionalField(nameof(Explode_On_Awake))]
    public float Explode_Damage = 200f;
    [ConditionalField(nameof(Explode_On_Awake))]
    public float Explode_Force = 1f;
    [ConditionalField(nameof(Explode_On_Awake))]
    public e_Entity_Side Side = e_Entity_Side.None;


    private void Awake() {
        Arrows_Mask = ~LayerMask.NameToLayer("Arrow");
        if(Explode_On_Awake) {
            Explode(Explode_Range, Explode_Damage, Explode_Force, Side);
        }
    }


    public void Explode(float range, float damage, float force, e_Entity_Side side) {
        StartCoroutine(Apply_Damage_And_Force(range, damage, force, side));
        Explode_Animation(range);
        GDG.Audio_Control.Play_Explode();
    }

    // ------------------

    private IEnumerator Apply_Damage_And_Force(float range, float damage, float force, e_Entity_Side side) {
        Collider[] hits = Physics.OverlapSphere(transform.position, range, Arrows_Mask);
        for(int i = 0; i < hits.Length; i++) {
            Apply_Damage(damage, hits[i], side);
        }
        yield return new WaitForFixedUpdate();
        hits = Physics.OverlapSphere(transform.position, range, Arrows_Mask);
        for(int i = 0; i < hits.Length; i++) {
            Apply_Force(range, force, hits[i]);
        }
    }

    private void Apply_Damage(float damage, Collider hit, e_Entity_Side side) {
        Damageable dmg_obj = hit.GetComponent<Damageable>();
        if(dmg_obj) {
            dmg_obj.Apply_Damage(new Damage(Damage.e_Damage_Type.Explosion, damage, this, side));
        }
    }

    private void Apply_Force(float range, float force, Collider hit) {
        Arrow arw = hit.GetComponent<Arrow>();
        if(arw == null) {
            Rigidbody rb = hit.attachedRigidbody;
            if(rb != null) {
                rb.AddExplosionForce(force, transform.position, range, 1f, ForceMode.Impulse);
            }
        }
    }

    // ------------------

    private void Explode_Animation(float range) {
        Explode_Paticles.Play();
        StartCoroutine(Delay_Destroy_Self());
        // Explosion_Sphere.localScale = Vector3.zero;
        // Sequence seq = DOTween.Sequence();
        // seq.Append(Explosion_Sphere.DOScale(Vector3.one * range * 2, 0.1f));
        // seq.Append(Explosion_Sphere.DOScale(Vector3.zero, 0.40f));
        // seq.AppendCallback(Destroy_Self);
    }

    private IEnumerator Delay_Destroy_Self() {
        yield return new WaitForSeconds(1f);
        Destroy_Self();
    }

    // ------------------

    public void Destroy_Self() {
        Destroy(gameObject);
    }

}
