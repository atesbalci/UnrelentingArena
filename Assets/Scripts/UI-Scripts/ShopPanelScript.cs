using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopPanelScript : MonoBehaviour {
    public GameObject skillGroupPrefab;
    public GameObject shopSkillPrefab;

    private GridLayoutGroup grid;

    void Awake() {
        grid = GetComponent<GridLayoutGroup>();
    }

    public void Refresh() {
        foreach (LayoutElement le in GetComponentsInChildren<LayoutElement>())
            Destroy(le.gameObject);
        PlayerData playerData = GameManager.instance.playerData;
        grid.cellSize = new Vector2(170, 460);
        Transform[] skillGroups = new Transform[4];
        for (int i = 0; i < 4; i++) {
            skillGroups[i] = ((GameObject)Instantiate(skillGroupPrefab)).transform;
            skillGroups[i].SetParent(gameObject.transform);
            skillGroups[i].GetComponentInChildren<Text>().text = "" + GameInput.instance.keys[(int)GameBinding.Skill1 + i];
        }
        foreach (KeyValuePair<SkillType, SkillPreset> kvp in playerData.skillSet.skills) {
            ShopSkillScript shopSkillScript = ((GameObject)Instantiate(shopSkillPrefab)).GetComponent<ShopSkillScript>();
            shopSkillScript.skillPreset = kvp.Value;
            shopSkillScript.gameObject.transform.SetParent(skillGroups[kvp.Value.key]);
        }
    }
}
