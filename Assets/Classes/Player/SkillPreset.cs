using UnityEngine;
using System.Collections;

public class SkillPreset {
    public SkillType skill { get; set; }
    public float damage { get; set; }
    public float range { get; set; }
    public float cooldown { get; set; }
    public float remainingCooldown { get; set; }
    public float channelTime { get; set; }
    public float recoilTime { get; set; }

    public SkillPreset(SkillType skill) {
        this.skill = skill;
        range = 10;
        damage = 20;
        cooldown = 5;
        remainingCooldown = 0;
        channelTime = 0.5f;
        recoilTime = 0.5f;
    }

    public void update() {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.deltaTime;
    }

    public string prefab {
        get {
            if (skill == SkillType.fireball)
                return "Fireball";
            else if (skill == SkillType.blink)
                return "Blink";
            return "";
        }
    }
}
