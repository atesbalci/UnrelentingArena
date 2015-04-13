using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScript : MonoBehaviour {
    public InputField nickname;

    void Awake() {
        if (GameInput.instance == null)
            GameInput.instance = new GameInput();
    }

    void OnEnable() {
        nickname.text = PlayerPrefs.GetString("name", "Player");
    }

    public void SetName() {
        PlayerPrefs.SetString("name", nickname.text);
        PlayerPrefs.Save();
    }
}
