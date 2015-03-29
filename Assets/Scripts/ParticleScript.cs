using UnityEngine;
using System.Collections;

public class ParticleScript : MonoBehaviour {
    private ParticleSystem myParticleSystem;
    private Quaternion rotation;

	void Start () {
        myParticleSystem = GetComponent<ParticleSystem>();
        rotation = GetComponentInParent<Transform>().rotation;
	}
	
	void Update () {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[256];
        myParticleSystem.GetParticles(particles);
        for (int i = 0; i < particles.Length; i++) {
            particles[i].axisOfRotation = Vector3.forward;
            particles[i].rotation = rotation.y;
        }
        myParticleSystem.SetParticles(particles, 256);
	}
}
