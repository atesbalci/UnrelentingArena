using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {
    public GameObject main;
    public GameObject settings;

    public void PlayButton() {
        Application.LoadLevel("MainGame");
    }

    private void NavigateTo(GameObject destination) {
        main.SetActive(false);
        settings.SetActive(false);

        destination.SetActive(true);
    }

    public void NavigateToMain() {
        NavigateTo(main);
    }

    public void NavigateToSettings() {
        NavigateTo(settings);
    }

    public void BackButton() {
        Application.LoadLevel("MainMenu");
    }

    public void ExitApp () {
        Application.Quit();
    }
}
