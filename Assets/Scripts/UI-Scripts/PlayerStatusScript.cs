using UnityEngine;
using System.Collections;

public class PlayerStatusScript : MonoBehaviour {
    private Quaternion defaultRotation;

    void Start() {
        defaultRotation = transform.rotation;
    }

    public void Update() {
        transform.rotation = defaultRotation;
    }
}
