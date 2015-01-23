using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {
    public const int FIREBALL = 0;
    public const int BLINK = 1;

    public Dictionary<int, SkillPreset> skills;

    public SkillSet() {
        skills = new Dictionary<int, SkillPreset>();
        SkillPreset sp = new SkillPreset(BLINK);
        skills.Add(BLINK, sp);
        sp = new SkillPreset(FIREBALL);
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
            if (sp.remainingCooldown < 0.01f)
                return sp;
        }
        return null;
    }
}
