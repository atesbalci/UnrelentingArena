using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CastBarInScript : MonoBehaviour {
    public Player player { get; set; }

    void Start() {

    }

    void Update() {
        if (player != null) {
            Channel channel = player.getChannel();
            if (channel != null) {
                GetComponent<Image>().fillAmount = 1 - channel.remainingDuration / channel.duration;
            }
        }
    }
}
