using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoresScript : MonoBehaviour {
    public GameObject listingPrefab;

    private float refreshTimer;

    void Start() {
        refreshTimer = 0;
    }

    void Update() {
        refreshTimer -= Time.deltaTime;
        if (refreshTimer <= 0) {
            refreshTimer = 1;
            RefreshPlayers();
            GameObject buttons = GetComponentInChildren<ScoresButtonScript>().gameObject;
            buttons.transform.SetParent(null);
            buttons.transform.SetParent(gameObject.transform);
        }
    }

    public void RefreshPlayers() {
        foreach (PlayerListingScript child in GetComponentsInChildren<PlayerListingScript>())
            Destroy(child.gameObject);
        foreach (PlayerData playerData in GameManager.instance.ListPlayers()) {
            GameObject playerListing = Instantiate(listingPrefab);
            playerListing.transform.SetParent(transform);
            playerListing.GetComponent<PlayerListingScript>().SetPlayer(playerData);
        }
    }
}
