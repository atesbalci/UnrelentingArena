using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ChangeShopScript : MonoBehaviour {
    public ShopPanelScript panel;

    private bool isSkills;

    void Start() {
        isSkills = false;
        Switch();
    }

    public void Switch() {
        isSkills = !isSkills;
        if (!isSkills)
            GetComponentInChildren<Text>().text = "Go to Skills";
        else
            GetComponentInChildren<Text>().text = "Go to Upgrades";
        panel.state = isSkills;
    }
}
