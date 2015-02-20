using UnityEngine;
using System.Collections;

public class StateNavigationScript : MonoBehaviour {
    public GameState targetState;

    public void Navigate() {
        Camera.main.GetComponent<GameManager>().state = targetState;
    }
}
