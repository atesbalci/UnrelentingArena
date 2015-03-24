using UnityEngine;
using System.Collections;

public class CameraSwitcherScript : MonoBehaviour {
    public GameObject game;
    public GameObject menu;

    private GameManager gameManager;

	void Start () {
        gameManager = GameManager.instance;
	}
	
	void Update () {
        if (gameManager.state == GameState.Ingame) {
            game.SetActive(true);
            menu.SetActive(false);
        } else {
            game.SetActive(false);
            menu.SetActive(true);
        }
	}
}
