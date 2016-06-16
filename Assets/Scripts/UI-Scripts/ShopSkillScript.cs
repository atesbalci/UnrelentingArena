using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

public class ShopSkillScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {
    public Image image;
    public Image glow;
    public SkillPreset skillPreset { get; set; }
    public bool unavailable { get; set; }
    public ShopPanelScript shopPanel { get; set; }

    public bool selected {
        get {
            return shopPanel.selected == this;
        }
        set {
            if (value)
                shopPanel.selected = this;
        }
    }

    public ShopSkillScript() {
        unavailable = false;
    }

    void Start() {
        Refresh();
    }

    public void Refresh() {
        if (skillPreset.level <= 0) {
            foreach (KeyValuePair<SkillType, SkillPreset> kvp in GameManager.instance.playerData.skillSet.skills) {
                if (skillPreset.key == kvp.Value.key && kvp.Value.level > 0)
                    unavailable = true;
            }
        }
        image.sprite = Resources.Load<Sprite>("Icons/" + skillPreset.name);
        if (image.sprite == null)
            image.sprite = Resources.Load<Sprite>("Icons/icon_template");
        float initialAlpha = glow.color.a;
        glow.color = skillPreset.level > 0 ? Color.cyan : Color.yellow;
        if (unavailable)
            glow.color = new Color(102.0f / 255.0f, 0f, 0f, 1.0f);
        glow.color = new Color(glow.color.r, glow.color.g, glow.color.b, initialAlpha);
    }

    public void Buy() {
        if (skillPreset != null)
            GameManager.instance.GetComponent<NetworkView>().RPC("UpgradeSkill", RPCMode.AllBuffered, Network.player, (int)skillPreset.skill);
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(true);
        GameManager.instance.tooltip.text.text = skillPreset.tooltip;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData) {
        if (!unavailable && skillPreset.level <= 0) {
            selected = true;
        }
    }

    void Update() {
        glow.color = Color.Lerp(glow.color, new Color(glow.color.r, glow.color.g, glow.color.b,
            (selected || unavailable || skillPreset.level > 0) ? 1 : 0), Time.deltaTime * 4);
    }
}
