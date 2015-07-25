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
            return "<b>Boomerang</b>\nMaterializes a remote controlled boomerang.";
        }
    }
}
