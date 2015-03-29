using UnityEngine;
using System.Collections;

public abstract class Buff {
    private float _duration;
    public float duration { get { return _duration; } set { _duration = value; remainingDuration = value; } }
    public float remainingDuration { get; set; }
    public Player player { get; set; }

    public Buff(Player player, float duration) {
        this.duration = duration;
        this.player = player;
    }

    public virtual void Update() {
        remainingDuration -= Time.deltaTime;
        if (remainingDuration <= 0) {
            player.RemoveBuff(this);
        }
    }

    public virtual void Unbuff() {
    }

    public virtual void ApplyBuff() {
    }
}
