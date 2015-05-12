using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class ShopPanelScript : MonoBehaviour {
    private bool _state;
    public bool state {
        get {
            return _state;
        }
        set {
            _state = value;
            Refresh();
        }
    }
    private GridLayoutGroup grid;

    void Awake() {
        grid = GetComponent<GridLayoutGroup>();
    }

    public void Refresh() {
        foreach (LayoutElement le in GetComponentsInChildren<LayoutElement>())
            Destroy(le.gameObject);
        PlayerData playerData = GameManager.instance.playerData;
        if (state) {
            grid.cellSize = new Vector2(170, 460);
            Transform[] skillGroups = new Transform[4];
            for (int i = 0; i < 4; i++) {
                skillGroups[i] = ((GameObject)Instantiate(Resources.Load("UI-Elements/SkillGroup"))).transform;
                skillGroups[i].SetParent(gameObject.transform);
                skillGroups[i].GetComponentInChildren<Text>().text = "" + GameInput.instance.keys[(int)GameBinding.Skill1 + i];
            }
            foreach (KeyValuePair<SkillType, SkillPreset> kvp in playerData.skillSet.skills) {
                ShopSkillScript shopSkillScript = ((GameObject)Instantiate(Resources.Load("UI-Elements/ShopSkill"))).GetComponent<ShopSkillScript>();
                shopSkillScript.skillPreset = kvp.Value;
                shopSkillScript.gameObject.transform.SetParent(skillGroups[kvp.Value.key]);
            }
        } else {
            grid.cellSize = new Vector2(150, 200);
            int no = 0;
            foreach (Item item in playerData.itemSet.potentialItems) {
                ShopItemScript shopItemScript = ((GameObject)Instantiate(Resources.Load("UI-Elements/ShopItem"))).GetComponent<ShopItemScript>();
                shopItemScript.item = item;
                shopItemScript.no = no;
                shopItemScript.gameObject.transform.SetParent(gameObject.transform);
                no++;
            }
        }
    }
}
