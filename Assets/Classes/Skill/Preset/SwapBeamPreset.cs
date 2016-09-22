using UnityEngine;
using System.Collections;

public class SwapBeamPreset : SkillPreset {
    public SwapBeamPreset()
        : base(SkillType.SwapBeam) {
        key = 2;
    }

    public override string name {
        get {
            return "Swap Beam";
        }
    }

    public override float cooldown {
        get {
            return 15 - (level * 1.5f);
        }
    }

    public override float damage {
        get {
            return 20 * level;
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
            return "<b>Swap Beam</b>\nSends a beam which makes the player swap places with the player hit.";
        }
    }
}
