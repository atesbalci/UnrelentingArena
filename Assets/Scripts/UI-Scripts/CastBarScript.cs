using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CastBarScript : MonoBehaviour {
    private Player player;

    void Update() {
        if (Camera.main.GetComponent<GameManager>().state == GameState.Ingame) {
            if (player != null) {
                Channel channel = player.Channel;
                if (channel != null) {
                    if (!channel.onRecoil) {
                        foreach (Image i in GetComponentsInChildren<Image>()) {
                            i.enabled = true;
                            i.fillAmount = 1 - channel.remainingDuration / channel.duration;
                        }
                        return;
                    }
                }
            } else {
                foreach (GameObject playerObject in GameObject.FindGameObjectsWithTag("Player")) {
                    PlayerScript playerScript = playerObject.GetComponent<PlayerScript>();
                    if (playerScript.owner == Network.player) {
                        player = playerScript.player;
                        break;
                    }
                }
            }
        } else {
            player = null;
        }
        foreach (Image i in GetComponentsInChildren<Image>())
            i.enabled = false;
    }
}
