using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//obselete class
public class ControlScript : MonoBehaviour {
    public KeyCode[] keys;
    public bool move { get; set; }
    public bool[] skills { get; set; }
    public bool block { get; set; }
    public bool mine { get; set; }

    void Start() {
        mine = false;
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
