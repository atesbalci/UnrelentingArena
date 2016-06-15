using UnityEngine;
using System.Collections;

public class IndefiniteBuff : Buff {
    public bool active { get; set; }

    public IndefiniteBuff(Player player) :
        base(player, 0) {
        active = false;
    }

    public override void Update() {
        if (!active)
            return;
    }
}
