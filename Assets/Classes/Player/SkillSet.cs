using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {
    public Dictionary<SkillType, SkillPreset> skills { get; set; }

    public SkillSet() {
        skills = new Dictionary<SkillType, SkillPreset>();
        skills.Add(SkillType.Fireball, new MeteorPreset());
        skills.Add(SkillType.Blink, new BlinkPreset());
        skills[SkillType.Blink].level = 1;
    }

    public void Update() {
        foreach (KeyValuePair<SkillType, SkillPreset> sp in skills) {
            sp.Value.Update();
        }
    }

    public void Upgrade(SkillType skill) {
        SkillPreset sp;
        if (skills.TryGetValue(skill, out sp)) {
            if (sp.maxLevel > sp.level)
                sp.level++;
        }
    }

    public SkillPreset TryToCast(SkillType skill) {
        if (skills.ContainsKey(skill)) {
            SkillPreset sp;
            skills.TryGetValue(skill, out sp);
            if (sp.level > 0 && sp.remainingCooldown < 0.01f)
                return sp;
        }
        return null;
    }

    public SkillPreset TryToCast(int key) {
        foreach (KeyValuePair<SkillType, SkillPreset> kvp in skills) {
            if (kvp.Value.key == key) {
                if (kvp.Value.remainingCooldown < 0.01f) {
                    return kvp.Value;
                }
                break;
            }
        }
        return null;
    }

    public int GetUpgradeCost(SkillType skill) {
        SkillPreset sp;
        if (skills.TryGetValue(skill, out sp)) {
            return sp.price;
        }
        return 0;
    }

    public LinkedList<SkillPreset> GetUnlockedSkills() {
        LinkedList<SkillPreset> result = new LinkedList<SkillPreset>();
        foreach (KeyValuePair<SkillType, SkillPreset> kvp in skills) {
            if (kvp.Value.level > 0) {
                result.AddLast(kvp.Value);
            }
        }
        return result;
    }
}
