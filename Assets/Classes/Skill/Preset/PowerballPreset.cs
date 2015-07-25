using UnityEngine;
using System.Collections;

public class PowerballPreset : SkillPreset {
    public PowerballPreset()
        : base(SkillType.Powerball) {
        key = 0;
    }

    public override string name {
        get {
            return "Powerball";
        }
    }

    public override float cooldown {
        get {
            return 10 - (level * 1.5f);
        }
    }

    public override float channelTime {
        get {
            return 0.3f;
        }
    }

    public override float recoilTime {
        get {
            return 0.05f;
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
            return "<b>Powerball</b>\nA simple ball of energy to damage and knock back your enemies on its path.";
        }
    }
}
