using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillsUI : MonoBehaviour {
    void OnEnable() {
        GameManager gameManager = GameManager.instance;
        SkillUIScript[] skills = GetComponentsInChildren<SkillUIScript>();
        LinkedList<SkillPreset> activeSkills = gameManager.playerData.skillSet.GetUnlockedSkills();
        foreach (SkillPreset skill in activeSkills) {
            skills[skill.key].key.text = "" + gameManager.keys[(int)GameBindings.Skill1 + skill.key];
            skills[skill.key].skill = skill;
        }
    }
}
