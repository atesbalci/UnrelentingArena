using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SkillsUI : MonoBehaviour {
    void OnEnable() {
        GameManager gameManager = GameManager.instance;
        SkillUIScript[] skills = GetComponentsInChildren<SkillUIScript>();
        LinkedList<SkillPreset> activeSkills = gameManager.playerData.skillSet.GetUnlockedSkills();
        KeyCode[] keys = gameManager.keys;
        foreach (SkillPreset skill in activeSkills) {
            skills[skill.key].key.text = "" + keys[skill.key];
            skills[skill.key].skill = skill;
        }
    }
}
