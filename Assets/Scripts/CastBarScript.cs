using UnityEngine;
using System.Collections;

public class CastBarScript : MonoBehaviour {
    private GameObject castBar;

    void Start() {
        castBar = GameObject.FindGameObjectWithTag("CastBar");
    }

    void Update() {
        Player player = null;
        GameObject[] playerQuery = GameObject.FindGameObjectsWithTag("Player");
        for (int i = 0; i < playerQuery.Length; i++) {
            if (playerQuery[i].GetComponent<NetworkView>() != null) {
                if (playerQuery[i].networkView.isMine) {
                    player = playerQuery[i].GetComponent<PlayerScript>().player;
                    break;
                }
            }
        }
        if (player != null) {
            Channel channel = player.getChannel();
            if (channel != null) {
                castBar.SetActive(!channel.onRecoil);
                if (!channel.onRecoil)
                    castBar.GetComponentInChildren<CastBarInScript>().player = player;
                return;
            }
        }
        castBar.SetActive(false);
    }
}
