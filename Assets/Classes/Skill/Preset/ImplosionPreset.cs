using UnityEngine;
using System.Collections;

public class ImplosionPreset : SkillPreset {
    public ImplosionPreset()
        : base(SkillType.Implosion) {
        key = 1;
    }

    public override string name {
        get {
            return "Implosion";
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
            return "<b>Implosion</b>\nCreates an implosion which pulls enemies.";
        }
    }
}
