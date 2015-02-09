using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void PlayButton() {
        Debug.Log("Selams");
        Application.LoadLevel("MainGame");
    }


    public void ExitApp () {
        Application.Quit();
    }


}
