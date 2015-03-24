using UnityEngine;
using System.Collections;

public class StateNavigationScript : MonoBehaviour {
    public GameState targetState;

    public void Navigate() {
        GameManager.instance.state = targetState;
    }
}
