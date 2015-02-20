using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ShopPanelScript : MonoBehaviour {
    public GameObject skills;
    public GameObject items;

    private bool isSkills;

    void Start() {
        isSkills = false;
        Switch();
    }

    public void Switch() {
        isSkills = !isSkills;
        if (isSkills)
            GetComponentInChildren<Text>().text = "Go to Skills";
        else
            GetComponentInChildren<Text>().text = "Go to Items";
        skills.SetActive(isSkills);
        items.SetActive(!isSkills);
    }
}
