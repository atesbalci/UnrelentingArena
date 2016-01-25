﻿using UnityEngine;
using System.Collections;

public class Powerball : SkillShot {
    private float time;
    private ParticleSystem[] particles;

    public Powerball()
        : base() {
        type = SkillType.Powerball;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        particles[0].startColor = player.color;
        particles[0].Stop();
        particles[0].Play();
        time = 2;
        if (modifier == ComboModifier.Momentum) {
            remainingDistance *= 0.5f;
            speed *= 2;
        } else if (modifier == ComboModifier.Fury) {
            remainingDistance *= 0.75f;
        }
    }

    public override void Update() {
        base.Update();
        if (maxRange)
            dead = true;
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        player.Damage(preset.damage, this.player);
        Vector3 direction = gameObject.transform.rotation * Vector3.forward;
        collider.gameObject.GetComponent<PlayerScript>().Knockback(direction, preset.knockbackDistance, preset.knockbackSpeed);
        dead = true;
    }

    public override void UpdateEnd() {
        foreach (ParticleSystem ps in particles)
            ps.Stop();
        if (time <= 0)
            Network.Destroy(gameObject);
        time -= Time.deltaTime;
    }
}
