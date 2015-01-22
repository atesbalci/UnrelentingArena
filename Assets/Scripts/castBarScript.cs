using UnityEngine;
using System.Collections;

public class CastBarScript : MonoBehaviour {
    private GameObject castBar;

    void Start() {
        castBar = GameObject.FindGameObjectWithTag("CastBar");
    }

    void Update() {
        Player player = null;
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
            player = go.GetComponent<PlayerScript>().player;
            break;
        }
        if (player != null) {
            Channel channel = player.getChannel();
            if (channel != null) {
                bool casting = channel.skill != null;
                castBar.SetActive(casting);
                if (casting)
                    castBar.GetComponentInChildren<CastBarInScript>().player = player;
                return;
            }
        }
        castBar.SetActive(false);
    }
}
