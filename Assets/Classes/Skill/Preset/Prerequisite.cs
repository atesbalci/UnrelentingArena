using UnityEngine;
using System.Collections;

public class Prerequisite {
    public SkillType skill { get; set; }
    public int level { get; set; }

    public Prerequisite(SkillType skill, int level) {
        this.skill = skill;
        this.level = level;
    }
}
