using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class ShopSkillScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    public Image image;
    public Text skillName;
    public Text level;
    public Button button;
    public SkillPreset skillPreset { get; set; }

    void Start() {
        Refresh();
    }

    public void Refresh() {
        skillName.text = skillPreset.name;
        level.text = "Level " + skillPreset.level;
        image.sprite = Resources.Load<Sprite>("Icons/" + skillPreset.name);
        button.GetComponentInChildren<Text>().text = skillPreset.price + " SP";
        button.interactable = skillPreset.level < skillPreset.maxLevel && skillPreset.available;
        if (!button.interactable) {
            if (skillPreset.available)
                button.GetComponentInChildren<Text>().text = "Max Level";
            else
                button.GetComponentInChildren<Text>().text = "N/A";
        }
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => { Buy(); });
        GameManager.instance.tooltip.gameObject.SetActive(true);
    }

    public void Buy() {
        if (skillPreset != null) {
            GameManager game = GameManager.instance;
            if (game.playerData.skillPoints >= skillPreset.price) {
                game.GetComponent<NetworkView>().RPC("UpgradeSkill", RPCMode.All, Network.player, (int)skillPreset.skill);
                GetComponentInParent<ShopPanelScript>().Refresh();
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(true);
        GameManager.instance.tooltip.text.text = skillPreset.tooltip;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(false);
    }
}
