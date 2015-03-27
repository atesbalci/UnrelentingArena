﻿using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {
    private ParticleSystem particleSystem;
    private Quaternion rotation;

	void Start () {
        particleSystem = GetComponent<ParticleSystem>();
        rotation = GetComponentInParent<Transform>().rotation;
	}
	
	void Update () {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[256];
        particleSystem.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++) {
            particles[i].axisOfRotation = Vector3.forward;
            particles[i].rotation = rotation.y;
        }
        particleSystem.SetParticles(particles, 256);
	}
}
