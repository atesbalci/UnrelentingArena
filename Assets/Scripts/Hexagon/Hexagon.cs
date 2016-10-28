using UnityEngine;
using System.Collections;

public class Hexagon : MonoBehaviour {
    public float targetHeight;

    public Transform trans { get; set; }

    private float currentHeight;

    void Start() {
        trans = transform;
        targetHeight = trans.localScale.y;
        currentHeight = trans.localScale.y;
    }

    public void Refresh() {
        if (Mathf.Abs(currentHeight - targetHeight) > 0.01f) {
            Vector3 scale = trans.localScale;
            currentHeight = Mathf.Lerp(scale.y, targetHeight, Time.deltaTime * 10);
            trans.localScale = new Vector3(scale.x, currentHeight, scale.z);
        }
    }
}
