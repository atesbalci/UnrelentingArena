using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AbilityUI : MonoBehaviour {
    public GameBinding type;

    void OnEnable() {
        GetComponentInChildren<Text>().text = "" + GameInput.instance.keys[(int)type];
    }
}
