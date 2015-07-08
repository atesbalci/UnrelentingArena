using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ResolutionsScript : MonoBehaviour {
    public Text resolutionText;
    public Toggle fullscreen;

    private Resolution[] resolutions;
    private int activeResolution;

    public ResolutionsScript() {
        resolutions = new Resolution[] {
            new Resolution(800, 600),
            new Resolution(1024, 768),
            new Resolution(1280, 720),
            new Resolution(1280, 800),
            new Resolution(1280, 1024),
            new Resolution(1360, 768),
            new Resolution(1366, 768),
            new Resolution(1440, 900),
            new Resolution(1600, 900),
            new Resolution(1600, 1200),
            new Resolution(1680, 1050),
            new Resolution(1920, 1200),
            new Resolution(2560, 1440),
            new Resolution(2560, 1600),
        };
    }

    void OnEnable() {
        resolutionText.text = Screen.width + "x" + Screen.height;
        fullscreen.isOn = Screen.fullScreen;
        activeResolution = 0;
        foreach (Resolution r in resolutions) {
            if (r.width == Screen.width && r.height == Screen.height)
                break;
            else
                activeResolution++;
        }
    }

    public void SetResolution() {
        int split = resolutionText.text.IndexOf('x');
        int width = int.Parse(resolutionText.text.Substring(0, split));
        int height = int.Parse(resolutionText.text.Substring(split + 1, resolutionText.text.Length - split - 1));
        Screen.SetResolution(width, height, fullscreen.isOn);
    }

    public void Switch(bool forward) {
        if (forward)
            activeResolution++;
        else
            activeResolution--;
        if (activeResolution >= resolutions.Length) {
            activeResolution = 0;
        }                     
        if(activeResolution < 0) {
            activeResolution = resolutions.Length - 1;
        }
        resolutionText.text = resolutions[activeResolution].width + "x" + resolutions[activeResolution].height;
    }

    private class Resolution {
        public int width { get; set; }
        public int height { get; set; }

        public Resolution(int w, int h) {
            width = w;
            height = h;
        }
    }
}
