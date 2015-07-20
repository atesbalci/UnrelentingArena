using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class SkillUIScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
    private SkillPreset _skill;
    public SkillPreset skill {
        get {
            return _skill;
        }
        set {
            _skill = value;
            string iconName = "Locked";
            if (skill != null) {
                iconName = skill.name;
            }
            image.sprite = Resources.Load<Sprite>("Icons/" + iconName);
        }
    }
    public Text key;
    public Text cooldown;
    public Image image;

    void Start() {
    }

    void Update() {
        cooldown.text = "";
        image.color = new Color(image.color.r, image.color.g, image.color.b, 1f);
        if (skill != null) {
            if (skill.remainingCooldown > 0) {
                cooldown.text = "" + skill.remainingCooldown.ToString("n1");
                image.color = new Color(image.color.r, image.color.g, image.color.b, 0.5f);
            }
        }
    }

    void OnDisable() {
        skill = null;
        key.text = "";
    }

    public void OnPointerEnter(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(true);
        GameManager.instance.tooltip.text.text = skill.tooltip;
    }

    public void OnPointerExit(PointerEventData eventData) {
        GameManager.instance.tooltip.gameObject.SetActive(false);
    }
}
