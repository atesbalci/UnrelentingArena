using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class NicknameFieldScript : MonoBehaviour {
    private InputField input;

    void Start() {
        input = GetComponentInParent<InputField>();
        if (GameManager.instance != null)
            input.text = GameManager.instance.playerData.name;
    }

    public void SetName() {
        string name = input.text;
        PlayerPrefs.SetString("name", name);
        PlayerPrefs.Save();
        if (GameManager.instance != null)
            GameManager.instance.playerData.name = name;
    }
}
