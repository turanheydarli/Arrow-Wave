using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable_Entity : Damageable {

    public GameObject Hide_On_Death;
    public GameObject Show_On_Death;
    private ParticleSystem Death_Particles;
    public string SFX_On_Death;

    private void Awake() {
        Death_Particles = GetComponentInChildren<ParticleSystem>();
    }

    protected override void Die() {
        if(Show_On_Death != null) {
            Show_On_Death.SetActive(true);
        }
        if(Hide_On_Death != null) {
            Hide_On_Death.SetActive(false);
        }
        Disable_Colliders();
        Death_Particles?.Play();
        GDG.Audio_Control.Play(SFX_On_Death);
    }

    private void Disable_Colliders() {
        Collider[] colliders = GetComponents<Collider>();
        for(int i = 0; i < colliders.Length; i++) {
            colliders[i].enabled = false;
        }
    }
}
