using UnityEngine;
using System.Collections;

public class CanvasNavigator : MonoBehaviour {
    public GameObject ingame;
    public GameObject scores;
    public GameObject menu;
    public GameObject shop;

    public void RefreshUI() {
        ingame.SetActive(false);
        scores.SetActive(false);
        menu.SetActive(false);
        shop.SetActive(false);
        GameState state = Camera.main.GetComponent<GameManager>().state;
        if (state == GameState.Ingame)
            ingame.SetActive(true);
        else if (state == GameState.Pregame || state == GameState.Scores)
            scores.SetActive(true);
        else if (state == GameState.Menu)
            menu.SetActive(true);
        else if (state == GameState.Shop)
            shop.SetActive(true);
    }
}
