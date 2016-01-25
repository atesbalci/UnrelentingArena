using UnityEngine;
using System.Collections;

public class MeteorPreset : SkillPreset {
    public MeteorPreset()
        : base(SkillType.Meteor) {
        key = 2;
    }

    public override string name {
        get {
            return "Meteor";
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
            return "<b>Meteor</b>\nSummons a virtual meteor to a nearby location.";
        }
    }
}
