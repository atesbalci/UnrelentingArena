using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopSkillScript : MonoBehaviour {
    public Image image;
    public Text skillName;
    public Text level;
    public Button button;
    public SkillPreset skillPreset { get; set; }

    void Start() {
        skillName.text = skillPreset.name;
        level.text = "Level " + skillPreset.level;
        button.interactable = skillPreset.level >= skillPreset.maxLevel;
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => { Buy(); });
        //image algorithm goes here
    }

    public void Buy() {
        if (skillPreset != null) {
            //if (Camera.main.GetComponent<GameManager>().playerData.credits)
            skillPreset.level++;
            Start();
        }
    }
}
