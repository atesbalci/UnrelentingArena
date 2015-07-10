using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipScript : MonoBehaviour {
    public Text text;

    void Update() {
        transform.position = new Vector3(Input.mousePosition.x + 5, Input.mousePosition.y - 5, Input.mousePosition.z);
    }
}
