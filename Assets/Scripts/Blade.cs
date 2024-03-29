﻿using UnityEngine;
using System.Collections;

public class Blade : MonoBehaviour {
    public SwingHitbox hitbox { get; set; }

    void Start() {
        foreach (TrailRenderer tr in GetComponentsInChildren<TrailRenderer>()) {
            tr.material.SetColor("_TintColor", hitbox.player.color);
        }
    }

    void OnEnable() {
        hitbox.detectCollision = 2;
    }
}
