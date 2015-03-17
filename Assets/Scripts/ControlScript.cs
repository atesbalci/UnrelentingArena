using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScript : MonoBehaviour {
    public KeyCode moveKey = KeyCode.Mouse1;

    public KeyCode[] keys;
    public bool move { get; set; }
    public bool[] skills { get; set; }
    public bool mine { get; set; }

    void Start() {
        mine = GetComponent<PlayerScript>().player.owner == Network.player;
        keys = Camera.main.GetComponent<GameManager>().keys;
        skills = new bool[8];
    }

    void Update() {
        if (mine) {
            move = Input.GetKey(moveKey);
            for (int i = 0; i < skills.Length; i++) {
                skills[i] = Input.GetKeyDown(keys[i]);
            }
        }
    }
}
