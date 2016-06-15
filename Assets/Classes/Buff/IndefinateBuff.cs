using UnityEngine;
using System.Collections;

public class IndefinateBuff : Buff {
    public bool active { get; set; }

    public IndefinateBuff(Player player) :
        base(player, 0) {
        active = false;
    }

    public override void Update() {
        if (!active)
            return;
    }
}
