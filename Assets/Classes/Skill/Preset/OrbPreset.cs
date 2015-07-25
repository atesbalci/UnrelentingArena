using UnityEngine;
using System.Collections;

public class OrbPreset : SkillPreset {
    public OrbPreset()
        : base(SkillType.Orb) {
        key = 2;
    }

    public override string name {
        get {
            return "Orb";
        }
    }

    public override float cooldown {
        get {
            return 10 - (level * 1.5f);
        }
    }

    public override float channelTime {
        get {
            return 1f;
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
            return "<b>Orb</b>\nSummons an orb to a nearby location which applies a momentary gravitational pull.";
        }
    }
}
