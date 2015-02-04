using UnityEngine;
using System.Collections;

public class Stun : Buff {
    public Stun(Player player)
        : base(player) {
    }

    public override void update() {
        player.currentSpeed = 0;
        player.canCast = false;
        base.update();
    }

    public override void debuff() {
        player.movementReset = true;
    }
}
