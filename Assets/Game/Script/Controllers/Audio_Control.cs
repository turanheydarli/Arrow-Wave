using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using MoreMountains.NiceVibrations;


public class Audio_Control : MonoBehaviour {

    private List<AudioSource> Audio_Sources;

    private bool Haptic_Continuous_Active = false;


    private void Awake() {
        AudioSource[] sounds = GetComponentsInChildren<AudioSource>();
        Audio_Sources = new List<AudioSource>(sounds);
    }

    // SFX -----------------------------

    public void Play(string name) {
        AudioSource sound = Get_Sound(name);
        sound?.Play();
    }

    public void Play(string name, float randomize_pitch) {
        AudioSource sound = Get_Sound(name);
        sound.pitch = Random.Range(1f - randomize_pitch, 1f + randomize_pitch);
        sound?.Play();
    }

    // Util ----------------------------

    private AudioSource Get_Sound(string name) {
        AudioSource sound = Audio_Sources.Find(x => x.name == name);
        return sound;
    }

    // SFX -----------------------------

    public void Play_Success() {
        Play("Success");
    }

    public void Play_Fail() {
        Play("Fail");
    }

    public void Play_Arrow_Pull() {
        Play("Arrow_Pull");
    }

    public void Play_Arrow_Shot() {
        Play("Arrow_Shot");
    }

    float Arrow_Impact_Last_Time = 0f;
    public void Play_Arrow_Impact() {
        if(Arrow_Impact_Last_Time + 0.1f > Time.realtimeSinceStartup) { return; }
        Arrow_Impact_Last_Time = Time.realtimeSinceStartup;
        Play("Arrow_Impact");
    }

    public void Play_Arrow_Hit() {
        Play("Arrow_Hit");
    }

    public void Play_Pop() {
        Play("Pop");
    }

    float Explode_Last_Time = 0f;
    public void Play_Explode() {
        if(Explode_Last_Time + 0.1f > Time.realtimeSinceStartup) { return; }
        Explode_Last_Time = Time.realtimeSinceStartup;
        Play("Explode");
    }


    public void Play_Enemy_Attack() {
        Play("Enemy_Attack");
    }

    //  Haptics ---------------------

    public void Play_Haptic_Heavy() {
        // MMVibrationManager.Haptic(HapticTypes.HeavyImpact, false, true, this);
    }


    public void Play_Haptic_Medium() {
        MMVibrationManager.Haptic(HapticTypes.MediumImpact, false, true, this);
    }

    public void Play_Haptic_Success() {
        MMVibrationManager.Haptic(HapticTypes.Success, false, true, this);
    }

    public void Play_Haptic_Failure() {
        MMVibrationManager.Haptic(HapticTypes.Failure, false, true, this);
    }

    // Continuous

    public void Play_Haptic_Continuous() {
        Haptic_Continuous_Active = true;
        StartCoroutine(Haptic_Continuous());
    }

    public void Stop_Haptic_Continuous() {
        Haptic_Continuous_Active = false;
    }

    IEnumerator Haptic_Continuous() {
        while(Haptic_Continuous_Active) {
            Play_Haptic_Medium();
            yield return new WaitForSeconds(0.12f);
        }
    }








}
