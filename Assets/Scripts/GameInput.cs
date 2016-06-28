using UnityEngine;
using System.Collections;

public enum GameBinding {
    Skill1 = 0,
    Skill2 = 1,
    Skill3 = 2,
    Skill4 = 3,
    Dodge = 4,
    Move = 5,
    Blade = 6
}

public class GameInput {
    public KeyCode[] keys { get; set; }
    public static GameInput instance;

    public GameInput() {
        keys = new KeyCode[7];
        LoadKeys();
    }

    public void LoadKeys() {
        keys[(int)GameBinding.Skill1] = (KeyCode)PlayerPrefs.GetInt("skill1key", (int)KeyCode.Q);
        keys[(int)GameBinding.Skill2] = (KeyCode)PlayerPrefs.GetInt("skill2key", (int)KeyCode.W);
        keys[(int)GameBinding.Skill3] = (KeyCode)PlayerPrefs.GetInt("skill3key", (int)KeyCode.E);
        keys[(int)GameBinding.Skill4] = (KeyCode)PlayerPrefs.GetInt("skill4key", (int)KeyCode.R);
        keys[(int)GameBinding.Dodge] = (KeyCode)PlayerPrefs.GetInt("dodgeKey", (int)KeyCode.LeftShift);
        keys[(int)GameBinding.Move] = (KeyCode)PlayerPrefs.GetInt("moveKey", (int)KeyCode.Mouse1);
        keys[(int)GameBinding.Blade] = (KeyCode)PlayerPrefs.GetInt("pulseKey", (int)KeyCode.Mouse0);
    }

    public void SaveKeys() {
        PlayerPrefs.SetInt("skill1key", (int)keys[(int)GameBinding.Skill1]);
        PlayerPrefs.SetInt("skill2key", (int)keys[(int)GameBinding.Skill2]);
        PlayerPrefs.SetInt("skill3key", (int)keys[(int)GameBinding.Skill3]);
        PlayerPrefs.SetInt("skill4key", (int)keys[(int)GameBinding.Skill4]);
        PlayerPrefs.SetInt("dodgeKey", (int)keys[(int)GameBinding.Dodge]);
        PlayerPrefs.SetInt("moveKey", (int)keys[(int)GameBinding.Move]);
        PlayerPrefs.SetInt("pulseKey", (int)keys[(int)GameBinding.Blade]);
        PlayerPrefs.Save();
    }
}
