using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public void playButton() {
        Debug.Log("Selams");
        Application.LoadLevel("MainGame");
    }


    public void exitApp () {
        Application.Quit();
    }


}
