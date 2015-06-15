using UnityEngine;
using System.Collections;

public class CameraSwitcherScript : MonoBehaviour {
    public GameObject game;
    public GameObject menu;
	
	void Update () {
        if (GameManager.instance.state == GameState.Ingame) {
            game.SetActive(true);
            menu.SetActive(false);
        } else {
            game.SetActive(false);
            menu.SetActive(true);
        }
	}
}
