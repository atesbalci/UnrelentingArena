using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class RoundInfoScript : MonoBehaviour {
	void Update () {
        if (Network.isServer || Network.isClient) {
            GetComponent<Image>().color = new Color(1, 1, 1, 0.39f);
            GetComponentInChildren<Text>().text = "Round: " + Camera.main.GetComponent<GameManager>().round;
        } else {
            GetComponent<Image>().color = new Color(1, 1, 1, 0);
            GetComponentInChildren<Text>().text = "";
        }
	}
}
