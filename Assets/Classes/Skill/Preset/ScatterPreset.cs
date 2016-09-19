using UnityEngine;
using System.Collections;

public class ScatterPreset : SkillPreset {
    public ScatterPreset()
        : base(SkillType.Scatter) {
        key = 0;
    }

    public override string name {
        get {
            return "Scatter Shot";
        }
    }

    public override float cooldown {
        get {
            return 12 - level;
        }
    }

    public override float damage {
        get {
            return 30 + 5 * level;
        }
    }

    public override float range {
        get {
            return 5 + level;
        }
    }

    public override int price {
        get {
            return 0 + ((level + 1) * 50);
        }
    }

    public override string tooltip {
        get {
            return "<b>Scatter Shot</b>\nSends out three spheres with great velocity and some spread.";
        }
    }
}
