using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MenuKeyBinder : MonoBehaviour {
    public GameBinding action;

    private bool _active;
    private bool active {
        get {
            return _active;
        }
        set {
            _active = value;
            if (active)
                GetComponentInChildren<Text>().color = Color.red;
            else
                GetComponentInChildren<Text>().color = Color.black;
        }
    }

    void Start() {
        Refresh();
    }

    void OnEnable() {
        active = false;
        if (GameInput.instance != null)
            Refresh();
    }

    public void Refresh() {
        GetComponentInChildren<Text>().text = GameInput.instance.keys[(int)action] + "";
    }

    public void Activate() {
        active = true;
    }

    void OnGUI() {
        if (active) {
            KeyCode key = GetPressedKey();
            if (key != KeyCode.None) {
                GameInput.instance.keys[(int)action] = key;
                GameInput.instance.SaveKeys();
                Refresh();
                active = false;
            }
        }
    }

    private KeyCode GetPressedKey() {
        int e = System.Enum.GetNames(typeof(KeyCode)).Length;
        for (int i = 0; i < e; i++) {
            if (Input.GetKey((KeyCode)i)) {
                return (KeyCode)i;
            }
        }
        return KeyCode.None;
    }
}
