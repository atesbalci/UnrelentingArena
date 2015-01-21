using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class castBarInScript : MonoBehaviour {

    void Start() {

    }

    void Update() {
        Channel channel = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerScript>().player.getChannel();
        if (channel != null) {
            GetComponent<Image>().fillAmount = 1 - channel.remainingDuration / channel.duration;
        }
    }
}
