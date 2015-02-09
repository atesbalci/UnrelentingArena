using UnityEngine;
using System.Collections;

public class Stun : Buff {
    public Stun(Player player)
        : base(player) {
    }

    public override void Update() {
        player.currentSpeed = 0;
        player.canCast = false;
        base.Update();
    }
}
