using UnityEngine;
using System.Collections;

public class PauseMenuScript : MonoBehaviour {
    public GameObject menu;

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
            Toggle();
    }

    public void Toggle() {
        menu.SetActive(!menu.activeSelf);
    }
}
