using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CastBarScript : MonoBehaviour {
    void Update() {
        Player player = Camera.main.GetComponent<GameManager>().player;
        if (player != null) {
            Channel channel = player.channel;
            if (channel != null) {
                if (!channel.onRecoil) {
                    foreach (Image i in GetComponentsInChildren<Image>()) {
                        i.enabled = true;
                        i.fillAmount = 1 - channel.remainingDuration / channel.duration;
                    }
                    return;
                }
            }
        }
        foreach (Image i in GetComponentsInChildren<Image>())
            i.enabled = false;
    }
}
