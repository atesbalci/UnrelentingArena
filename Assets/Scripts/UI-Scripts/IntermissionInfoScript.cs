using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class IntermissionInfoScript : MonoBehaviour {
    void Update() {
        GameManager gameManager = GameManager.instance;
        if (gameManager.state == GameState.Scores || gameManager.state == GameState.Shop) {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.39f);
            GetComponentInChildren<Text>().text = "Intermission: " + Mathf.RoundToInt(gameManager.remainingIntermissionDuration);
        } else {
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
            GetComponentInChildren<Text>().text = "";
        }
    }
}
