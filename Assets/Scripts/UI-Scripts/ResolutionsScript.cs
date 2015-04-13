using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionsScript : MonoBehaviour {
    public InputField width;
    public InputField height;
    public Toggle fullscreen;

    void OnEnable() {
        width.text = Screen.width + "";
        height.text = Screen.height + "";
        fullscreen.isOn = Screen.fullScreen;
    }

    public void SetResolution() {
        Screen.SetResolution(int.Parse(width.text), int.Parse(height.text), fullscreen.isOn);
    }
}
