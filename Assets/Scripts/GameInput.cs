using UnityEngine;
using System.Collections;

public enum GameBinding {
    Skill1 = 0,
    Skill2 = 1,
    Skill3 = 2,
    Skill4 = 3,
    Skill5 = 4,
    Skill6 = 5,
    Skill7 = 6,
    Skill8 = 7,
    Block = 8,
    Move = 9
}

public class GameInput {
    public KeyCode[] keys { get; set; }
    public static GameInput instance;

    public GameInput() {
        keys = new KeyCode[10];
        LoadKeys();
    }

    public void LoadKeys() {
        keys[(int)GameBinding.Skill1] = (KeyCode)PlayerPrefs.GetInt("skill1key", (int)KeyCode.Q);
        keys[(int)GameBinding.Skill2] = (KeyCode)PlayerPrefs.GetInt("skill2key", (int)KeyCode.W);
        keys[(int)GameBinding.Skill3] = (KeyCode)PlayerPrefs.GetInt("skill3key", (int)KeyCode.E);
        keys[(int)GameBinding.Skill4] = (KeyCode)PlayerPrefs.GetInt("skill4key", (int)KeyCode.R);
        keys[(int)GameBinding.Skill5] = (KeyCode)PlayerPrefs.GetInt("skill5key", (int)KeyCode.A);
        keys[(int)GameBinding.Skill6] = (KeyCode)PlayerPrefs.GetInt("skill6key", (int)KeyCode.S);
        keys[(int)GameBinding.Skill7] = (KeyCode)PlayerPrefs.GetInt("skill7key", (int)KeyCode.D);
        keys[(int)GameBinding.Skill8] = (KeyCode)PlayerPrefs.GetInt("skill8key", (int)KeyCode.F);
        keys[(int)GameBinding.Block] = (KeyCode)PlayerPrefs.GetInt("blockKey", (int)KeyCode.LeftShift);
        keys[(int)GameBinding.Move] = (KeyCode)PlayerPrefs.GetInt("moveKey", (int)KeyCode.Mouse1);
    }

    public void SaveKeys() {
        PlayerPrefs.SetInt("skill1key", (int)keys[(int)GameBinding.Skill1]);
        PlayerPrefs.SetInt("skill2key", (int)keys[(int)GameBinding.Skill2]);
        PlayerPrefs.SetInt("skill3key", (int)keys[(int)GameBinding.Skill3]);
        PlayerPrefs.SetInt("skill4key", (int)keys[(int)GameBinding.Skill4]);
        PlayerPrefs.SetInt("skill5key", (int)keys[(int)GameBinding.Skill5]);
        PlayerPrefs.SetInt("skill6key", (int)keys[(int)GameBinding.Skill6]);
        PlayerPrefs.SetInt("skill7key", (int)keys[(int)GameBinding.Skill7]);
        PlayerPrefs.SetInt("skill8key", (int)keys[(int)GameBinding.Skill8]);
        PlayerPrefs.SetInt("blockKey", (int)keys[(int)GameBinding.Block]);
        PlayerPrefs.SetInt("moveKey", (int)keys[(int)GameBinding.Move]);
        PlayerPrefs.Save();
    }
}
