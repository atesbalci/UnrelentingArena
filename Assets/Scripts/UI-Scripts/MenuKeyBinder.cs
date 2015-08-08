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
            GetComponent<Button>().interactable = !active;
            if (active)
                GetComponentInChildren<Text>().color = Color.red;
            else
                GetComponentInChildren<Text>().color = Color.white;
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
        if (active && Input.anyKey) {
            KeyCode key = GetPressedKey();
            if (key != KeyCode.None) {
                if (key == KeyCode.Escape)
                    active = false;
                else if (key == KeyCode.Mouse0 && UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
                    return;
                else {
                    GameInput.instance.keys[(int)action] = key;
                    GameInput.instance.SaveKeys();
                    Refresh();
                    active = false;
                }
            }
        }
    }

    private KeyCode GetPressedKey() {
        for (int i = 0; i < 510; i++) {
            if (Input.GetKey((KeyCode)i)) {
                return (KeyCode)i;
            }
            if (i == 128)
                i = 255;
        }
        return KeyCode.None;
    }
}
