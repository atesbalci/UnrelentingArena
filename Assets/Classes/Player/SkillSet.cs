using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {
    public Dictionary<SkillType, SkillPreset> skills;

    public SkillSet() {
        skills = new Dictionary<SkillType, SkillPreset>();
        SkillPreset sp = new SkillPreset(SkillType.blink);
        skills.Add(SkillType.blink, sp);
        sp = new SkillPreset(SkillType.fireball);
        skills.Add(SkillType.fireball, sp);
    }

    public void update() {
        foreach (KeyValuePair<SkillType, SkillPreset> sp in skills) {
            sp.Value.update();
        }
    }

    public SkillPreset tryToCast(SkillType skill) {
        if (skills.ContainsKey(skill)) {
            SkillPreset sp;
            skills.TryGetValue(skill, out sp);
            if (sp.remainingCooldown < 0.01f)
                return sp;
        }
        return null;
    }
}
