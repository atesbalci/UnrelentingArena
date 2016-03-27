using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoresScript : MonoBehaviour {
	public GameObject listingPrefab;

	private float refreshTimer;
	private Button start;

	void Start() {
		refreshTimer = 0;
		start = GetComponentInChildren<Button>();
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
		foreach (PlayerData playerData in GameManager.instance.ListPlayers()) {
			GameObject playerListing = Instantiate(listingPrefab);
			playerListing.transform.SetParent(transform);
			playerListing.GetComponent<PlayerListingScript>().SetPlayer(playerData);
		}
		start.gameObject.transform.SetParent(null);
		start.gameObject.transform.SetParent(transform);
		if (GameManager.instance.state == GameState.Pregame && Network.isServer)
			start.gameObject.SetActive(true);
		else
			start.gameObject.SetActive(false);
	}
}
