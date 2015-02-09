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
    public int level { get; set; }

    public SkillPreset(SkillType skill) {
        this.skill = skill;
        range = 10;
        damage = 20;
        cooldown = 5;
        remainingCooldown = 0;
        channelTime = 0.5f;
        recoilTime = 0.5f;
        level = 1;
    }

    public void Update() {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.deltaTime;
    }

    public string prefab {
        get {
            if (skill == SkillType.Fireball)
                return "Fireball";
            else if (skill == SkillType.Blink)
                return "Blink";
            return "";
        }
    }
}
