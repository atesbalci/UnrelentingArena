using UnityEngine;
using System.Collections;

public abstract class SkillPreset {
    public SkillType skill { get; set; }
    public float remainingCooldown { get; set; }
    public int level { get; set; }
    public int key { get; set; }
    public bool available { get; set; }

    public SkillPreset(SkillType skill) {
        this.skill = skill;
        remainingCooldown = 0;
        level = 0;
        available = true;
    }

    public void Update() {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.deltaTime;
    }

    public virtual float cooldown {
        get {
            return 0;
        }
    }

    public virtual float channelTime {
        get {
            return 0;
        }
    }

    public virtual float recoilTime {
        get {
            return 0;
        }
    }

    public virtual float damage {
        get {
            return 0;
        }
    }

    public virtual float range {
        get {
            return 0;
        }
    }

    public virtual string name {
        get {
            return "";
        }
    }

    public virtual int maxLevel {
        get {
            return 5;
        }
    }

    public virtual Prerequisite prerequisite {
        get {
            return new Prerequisite(SkillType.None, 0);
        }
    }

    public virtual int price {
        get {
            return 0;
        }
    }

    public virtual string tooltip {
        get {
            return "<b>N/A</b>\nN/A";
        }
    }
}
