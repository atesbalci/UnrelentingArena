using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {
    private const int FIREBALL = 0;
    private const int BLINK = 1;

    public Dictionary<int, SkillPreset> skills;

    public SkillSet() {
        skills = new Dictionary<int, SkillPreset>();
        SkillPreset sp = new SkillPreset();
        skills.Add(BLINK, sp);
        sp = new SkillPreset();
        skills.Add(FIREBALL, sp);
    }

    public void update() {
        foreach (KeyValuePair<int, SkillPreset> sp in skills) {
            sp.Value.update();
        }
    }

    public SkillPreset tryToCast(int skillName) {
        if (skills.ContainsKey(skillName)) {
            SkillPreset sp;
            skills.TryGetValue(skillName, out sp);
            if (sp.remainingCooldown < 0.01f) {
                sp.remainingCooldown = sp.cooldown;
                return sp;
            }
        }
        return null;
    }

    public Fireball castFireball(Player player) {
        SkillPreset sp = tryToCast(FIREBALL);
        if (sp != null) {
            Fireball fireball = new Fireball(player);
            fireball.damage = sp.damage;
            fireball.range = sp.range;
            return fireball;
        }
        return null;
    }

    public Blink castBlink(Player player) {
        SkillPreset sp = tryToCast(BLINK);
        if (sp != null) {
            Blink blink = new Blink(player);
            blink.range = sp.range;
            return blink;
        }
        return null;
    }
}
