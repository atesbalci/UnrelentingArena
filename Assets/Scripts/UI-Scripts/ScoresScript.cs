using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoresScript : MonoBehaviour {
    private float refreshTimer;

    void Start() {
        refreshTimer = 0;
    }

    void Update() {
        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0) {
            refreshTimer = 1;
            RefreshPlayers();
        }
    }

    public void RefreshPlayers() {
        foreach (PlayerListingScript child in GetComponentsInChildren<PlayerListingScript>())
            Destroy(child.gameObject);
        int y = -15;
        foreach (PlayerData playerData in Camera.main.GetComponent<GameManager>().ListPlayers()) {
            GameObject playerListing = GameObject.Instantiate(Resources.Load("UI-Elements/PlayerListing")) as GameObject;
            playerListing.transform.SetParent(transform);
            playerListing.GetComponent<PlayerListingScript>().SetPlayer(playerData);
            playerListing.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, y);
            y -= 30;
        }
        GetComponent<RectTransform>().sizeDelta = new Vector3(GetComponent<RectTransform>().rect.width, -y + 15);
    }
}
