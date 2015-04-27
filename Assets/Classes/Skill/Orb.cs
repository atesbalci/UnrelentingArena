using UnityEngine;
using System.Collections;

public class Orb : Skill {
    private enum OrbState {
        Rising, Falling, Landed, Damaging, Ending
    }

    private Renderer area;
    private OrbState state;

    public Orb() {
        state = OrbState.Rising;
    }

    public override void Start(GameObject gameObject) {
        area = gameObject.GetComponentsInChildren<Renderer>()[1];
    }


}
