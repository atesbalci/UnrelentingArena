using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ControlScript : MonoBehaviour {
    public KeyCode[] keys;
    public bool move { get; set; }
    public bool[] skills { get; set; }
    public bool block { get; set; }
    public bool mine { get; set; }

    void Start() {
        mine = GetComponent<PlayerScript>().player.owner == Network.player;
        skills = new bool[8];
        keys = GameInput.instance.keys;
    }

    void Update() {
        if (mine) {
            move = Input.GetKey(keys[(int)GameBinding.Move]);
            for (int i = 0; i < skills.Length; i++) {
                skills[i] = Input.GetKey(keys[(int)GameBinding.Skill1 + i]);
            }
        }
    }
}
