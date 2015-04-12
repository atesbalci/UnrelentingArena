using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NicknameFieldScript : MonoBehaviour {
    private InputField input;

    void Start() {
        input = GetComponentInParent<InputField>();
        input.text = GameManager.instance.playerData.name;
    }

    public void SetName() {
        string name = input.text;
        GameManager.instance.playerData.name = name;
        PlayerPrefs.SetString("name", name);
        PlayerPrefs.Save();
    }
}
