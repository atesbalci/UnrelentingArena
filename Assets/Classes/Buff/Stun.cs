using UnityEngine;
using System.Collections;

public class Stun : Buff {
    public Stun(Player player, float duration)
        : base(player, duration) {
    }

    public override void Update() {
        player.currentSpeed = 0;
        player.canCast = false;
        base.Update();
    }

    public override void ApplyBuff() {
        player.RemoveBuff(player.castChannel);
    }
}
