using UnityEngine;
using System.Collections;

public class MinePreset : SkillPreset {
    public MinePreset()
        : base(SkillType.Mine) {
        key = 1;
    }

    public override string name {
        get {
            return "Mine";
        }
    }

    public override float cooldown {
        get {
            return 15 - (level * 1.5f);
        }
    }

    public override float channelTime {
        get {
            return 0.05f;
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
            return "<b>Mine</b>\nPlaces a stunning mine.";
        }
    }
}
