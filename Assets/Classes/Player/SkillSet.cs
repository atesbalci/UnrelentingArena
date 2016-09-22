using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {
    public Dictionary<SkillType, SkillPreset> skills { get; set; }

    public SkillSet() {
        skills = new Dictionary<SkillType, SkillPreset>();
        skills.Add(SkillType.Powerball, new PowerballPreset());
        skills.Add(SkillType.SwapBeam, new SwapBeamPreset());
        skills.Add(SkillType.Charge, new ChargePreset());
        skills.Add(SkillType.Overcharge, new OverchargePreset());
        skills.Add(SkillType.Blink, new BlinkPreset());
        skills.Add(SkillType.StunArea, new StunAreaPreset());
        skills.Add(SkillType.Scatter, new ScatterPreset());
        skills.Add(SkillType.Implosion, new ImplosionPreset());

        //Upgrade(SkillType.Powerball);
        //Upgrade(SkillType.SwapBeam);
        //Upgrade(SkillType.Implosion);
        //Upgrade(SkillType.Blink);
        Upgrade(SkillType.StunArea);
        //Upgrade(SkillType.Overcharge);
        //Upgrade(SkillType.Scatter);
        //Upgrade(SkillType.Charge);
    }

    public void Update() {
        foreach (KeyValuePair<SkillType, SkillPreset> sp in skills) {
            sp.Value.Update();
        }
    }

    public void Upgrade(SkillType skill) {
        SkillPreset sp = skills[skill];
        if (sp.maxLevel > sp.level) {
            sp.level++;
            foreach (var kvp in skills) {
                if (kvp.Value.key == sp.key && kvp.Value != sp)
                    kvp.Value.available = false;
            }
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
            if (kvp.Value.key == key && kvp.Value.level > 0) {
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
