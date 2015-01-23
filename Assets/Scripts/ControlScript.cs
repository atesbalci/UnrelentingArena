using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScript : MonoBehaviour {
    public KeyCode moveKey = KeyCode.Mouse1;
    public KeyCode spell1Key = KeyCode.Alpha1;
    public KeyCode spell2Key = KeyCode.Alpha2;

    public bool move;
    public bool spell1;
    public bool spell2;

    private bool mine;

    void Start() {
        mine = networkView.isMine;
    }

    void Update() {
        if (mine) {
            move = Input.GetKey(moveKey);
            spell1 = Input.GetKeyDown(spell1Key);
            spell2 = Input.GetKeyDown(spell2Key);
        }
    }
}
