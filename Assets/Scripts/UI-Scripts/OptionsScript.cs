using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OptionsScript : MonoBehaviour {
    public InputField nickname;
    public InputField width;
    public InputField height;
    public Toggle fullscreen;

    void Awake() {
        if (GameInput.instance == null)
            GameInput.instance = new GameInput();
    }

    void OnEnable() {
        nickname.text = PlayerPrefs.GetString("name", "Player");
        width.text = Screen.width + "";
        height.text = Screen.height + "";
        fullscreen.isOn = Screen.fullScreen;
    }

    public void SetResolution() {
        Screen.SetResolution(int.Parse(width.text), int.Parse(height.text), fullscreen.isOn);
    }

    public void SetName() {
        PlayerPrefs.SetString("name", nickname.text);
        PlayerPrefs.Save();
    }
}
