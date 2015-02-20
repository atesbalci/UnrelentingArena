using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoresButtonScript : MonoBehaviour {
    public GameObject startButton;
    public GameObject shopButton;

    void OnEnable() {
        startButton.SetActive(false);
        shopButton.SetActive(false);
        GameManager gameManager = Camera.main.GetComponent<GameManager>();
        if (gameManager.state == GameState.Scores)
            shopButton.SetActive(true);
        else if (gameManager.state == GameState.Pregame && Network.isServer)
            startButton.SetActive(true);
    }
}
