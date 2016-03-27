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
        GameState state = GameManager.instance.state;
        if (state == GameState.Ingame)
            ingame.SetActive(true);
        else if (state == GameState.Pregame || state == GameState.Intermission)
            scores.SetActive(true);
        else if (state == GameState.Menu)
            menu.SetActive(true);
        if (state == GameState.Intermission)
            shop.SetActive(true);
    }
}
