using UnityEngine;
using System.Collections;

public class SkillPreset {
    public float damage { get; set; }
    public float range { get; set; }
    public float cooldown { get; set; }
    public float remainingCooldown { get; set; }

    public SkillPreset() {
        range = 10;
        damage = 20;
        cooldown = 5;
        remainingCooldown = 0;
    }

    public void update() {
        if (remainingCooldown > 0)
            remainingCooldown -= Time.deltaTime;
    }
}
