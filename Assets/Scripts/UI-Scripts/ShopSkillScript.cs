﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopSkillScript : MonoBehaviour {
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
        button.GetComponentInChildren<Text>().text = skillPreset.price + "C";
        button.interactable = skillPreset.level < skillPreset.maxLevel;
        button.onClick = new Button.ButtonClickedEvent();
        button.onClick.AddListener(() => { Buy(); });
        //image algorithm goes here
    }

    public void Buy() {
        if (skillPreset != null) {
            GameManager game = Camera.main.GetComponent<GameManager>();
            if (game.playerData.credits >= skillPreset.price) {
                game.networkView.RPC("UpgradeSkill", RPCMode.All, Network.player, (int)skillPreset.skill);
                Refresh();
            }
        }
    }
}
