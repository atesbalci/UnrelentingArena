using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class CastBarScript : MonoBehaviour {
    public Image castbarIn;
    public Image castbarBack;

    private Player player;

    void OnEnable() {
        player = GameManager.instance.playerData.currentPlayer;
    }

    void Update() {
        if (player != null) {
            CastChannel channel = player.castChannel;
            if (channel != null) {
                castbarBack.enabled = true;
                castbarIn.enabled = true;
                castbarIn.fillAmount = 1 - channel.remainingDuration / channel.duration;
                return;
            }
        }
        castbarIn.enabled = false;
        castbarBack.enabled = false;
    }
}
