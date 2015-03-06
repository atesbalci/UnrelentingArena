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

    public void Refresh() {
        foreach (LayoutElement le in GetComponentsInChildren<LayoutElement>())
            Destroy(le.gameObject);
        PlayerData playerData = Camera.main.GetComponent<GameManager>().playerData;
        if (state) {
            foreach (KeyValuePair<SkillType, SkillPreset> kvp in playerData.skillSet.skills) {
                ShopSkillScript shopSkillScript = ((GameObject)Instantiate(Resources.Load("UI-Elements/ShopSkill"))).GetComponent<ShopSkillScript>();
                shopSkillScript.skillPreset = kvp.Value;
                shopSkillScript.gameObject.transform.SetParent(gameObject.transform);
                Debug.Log("sadsdasda");
            }
        } else {

        }
    }
}
