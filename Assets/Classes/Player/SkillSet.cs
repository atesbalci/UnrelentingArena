using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {
    public Dictionary<SkillType, SkillPreset> skills;

    public SkillSet() {
        skills = new Dictionary<SkillType, SkillPreset>();
        SkillPreset sp = new SkillPreset(SkillType.Blink);
        skills.Add(SkillType.Blink, sp);
        sp = new SkillPreset(SkillType.Fireball);
        skills.Add(SkillType.Fireball, sp);
    }

    public void Update() {
        foreach (KeyValuePair<SkillType, SkillPreset> sp in skills) {
            sp.Value.Update();
        }
    }

    public SkillPreset TryToCast(SkillType skill) {
        if (skills.ContainsKey(skill)) {
            SkillPreset sp;
            skills.TryGetValue(skill, out sp);
            if (sp.remainingCooldown < 0.01f)
                return sp;
        }
        return null;
    }
}
