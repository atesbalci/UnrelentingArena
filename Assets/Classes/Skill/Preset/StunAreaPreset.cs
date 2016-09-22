using UnityEngine;
using System.Collections;

public class StunAreaPreset : SkillPreset {
    public StunAreaPreset()
        : base(SkillType.StunArea) {
        key = 2;
    }

    public override string name {
        get {
            return "Stun Area";
        }
    }

    public override float cooldown {
        get {
            return 10 - (level * 1.5f);
        }
    }

    public override float damage {
        get {
            return 40 + 5 * level;
        }
    }

    public override float range {
        get {
            return 10 + (level * 2);
        }
    }

    public override int price {
        get {
            return 0 + ((level + 1) * 50);
        }
    }

    public override string tooltip {
        get {
            return "<b>Stun Area</b>\nStuns players within a limited area.";
        }
    }
}
