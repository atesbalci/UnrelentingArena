using UnityEngine;
using System.Collections;

public class BoomerangPreset : SkillPreset {
    public BoomerangPreset()
        : base(SkillType.Boomerang) {
        key = 0;
    }

    public override string name {
        get {
            return "Boomerang";
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
            return 10 + level;
        }
    }

    public override int price {
        get {
            return 0 + ((level + 1) * 50);
        }
    }

    public override string tooltip {
        get {
            return "<b>Boomerang</b>\nMaterializes a remote controlled boomerang.";
        }
    }
}
