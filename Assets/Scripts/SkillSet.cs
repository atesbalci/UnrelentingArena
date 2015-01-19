using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SkillSet {

    public SkillSet() {
        
    }

    public Skill castFireball() {
        Skill skill = new Skill();
        skill.setPrefab("Fireball");
        return skill;
    }
}
