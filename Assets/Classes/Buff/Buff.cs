﻿using UnityEngine;
using System.Collections;

public class Buff {
    private float _duration;
    public float duration { get { return _duration; } set { _duration = value; remainingDuration = value; } }
    public float remainingDuration { get; set; }
    public Player player { get; set; }

    public Buff(Player player) {
        this.player = player;
    }

    public virtual void update() {
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0) {
            player.removeBuff(this);
        }
    }

    public virtual void debuff() {
    }

    public virtual void buff() {
    }
}