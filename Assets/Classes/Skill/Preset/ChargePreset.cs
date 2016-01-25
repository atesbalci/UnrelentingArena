using UnityEngine;
using System.Collections;

public class ChargePreset : SkillPreset {
    public ChargePreset()
        : base(SkillType.Charge) {
        key = 3;
    }

    public override string name {
        get {
            return "Charge";
        }
    }

    public override float cooldown {
        get {
            return 2 - (level * 1.5f);
        }
    }

    public override float damage {
        get {
            return 15 + level * 5;
        }
    }

    public override float range {
        get {
            return 4 + level;
        }
    }

    public override int price {
        get {
            return 0 + ((level + 1) * 50);
        }
    }

    public override string tooltip {
        get {
            return "<b>Charge</b>\nCharge to a location with pace, while knocking back anything on destination.";
        }
    }
}
