using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopPanelScript : MonoBehaviour {
	public GameObject skillGroupPrefab;
	public GameObject shopSkillPrefab;

	public ShopSkillScript[] skills { get; set; }
	public ShopSkillScript selected { get; set; }

	void OnEnable() {
		Refresh();
	}

	public void Refresh() {
		foreach (LayoutElement le in GetComponentsInChildren<LayoutElement>())
			Destroy(le.gameObject);
		PlayerData playerData = GameManager.instance.playerData;
		Transform[] skillGroups = new Transform[4];
		for (int i = 0; i < 4; i++) {
			skillGroups[i] = ((GameObject)Instantiate(skillGroupPrefab)).transform;
			skillGroups[i].SetParent(gameObject.transform);
			skillGroups[i].GetComponentInChildren<Text>().text = "" + GameInput.instance.keys[(int)GameBinding.Skill1 + i];
		}
		List<ShopSkillScript> skillsList = new List<ShopSkillScript>();
		foreach (KeyValuePair<SkillType, SkillPreset> kvp in playerData.skillSet.skills) {
			ShopSkillScript shopSkill = ((GameObject)Instantiate(shopSkillPrefab)).GetComponent<ShopSkillScript>();
			shopSkill.shopPanel = this;
			shopSkill.skillPreset = kvp.Value;
			shopSkill.gameObject.transform.SetParent(skillGroups[kvp.Value.key]);
			int i;
			for (i = 0; i < skillsList.Count; i++) {
				if (shopSkill.skillPreset.key < skillsList[i].skillPreset.key)
					break;
			}
			skillsList.Insert(i, shopSkill);
		}
		skills = skillsList.ToArray();
		selected = null;
		foreach (ShopSkillScript skill in skills) {
			if (!skill.unavailable && skill.skillPreset.level <= 0) {
				selected = skill;
				break;
			}
		}
	}

    void OnDisable() {
        selected.Buy();
    }
}
