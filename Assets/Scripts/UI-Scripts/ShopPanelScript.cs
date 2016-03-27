using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopPanelScript : MonoBehaviour {
    public GameObject skillGroupPrefab;
    public GameObject shopSkillPrefab;

	public List<ShopSkillScript> skills { get; set;	}

	void OnEnable() {
		Refresh();
	}

    public void Refresh() {
		skills = new List<ShopSkillScript>();
        foreach (LayoutElement le in GetComponentsInChildren<LayoutElement>())
            Destroy(le.gameObject);
        PlayerData playerData = GameManager.instance.playerData;
        Transform[] skillGroups = new Transform[4];
        for (int i = 0; i < 4; i++) {
            skillGroups[i] = ((GameObject)Instantiate(skillGroupPrefab)).transform;
            skillGroups[i].SetParent(gameObject.transform);
            skillGroups[i].GetComponentInChildren<Text>().text = "" + GameInput.instance.keys[(int)GameBinding.Skill1 + i];
        }
        foreach (KeyValuePair<SkillType, SkillPreset> kvp in playerData.skillSet.skills) {
            ShopSkillScript shopSkill = ((GameObject)Instantiate(shopSkillPrefab)).GetComponent<ShopSkillScript>();
			shopSkill.shopPanel = this;
			shopSkill.skillPreset = kvp.Value;
            shopSkill.gameObject.transform.SetParent(skillGroups[kvp.Value.key]);
			skills.Add(shopSkill);
        }
    }
}
