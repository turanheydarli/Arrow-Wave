using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Confetti_Control : MonoBehaviour {
    public ParticleSystem Confetti_Particles;

    public void Play_Confetti() {
        Confetti_Particles.Clear();
        Confetti_Particles.Play();
    }

}
