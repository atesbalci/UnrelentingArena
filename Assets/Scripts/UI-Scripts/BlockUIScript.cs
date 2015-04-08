using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class BlockUIScript : MonoBehaviour {
    public Image image;

    private Player player;
    private ShieldScript shieldScript;
    private const float FULL = 0.235f;

    void Start() {
        player = GetComponentInParent<PlayerScript>().player;
        shieldScript = GetComponentInParent<ShieldScript>();
    }

    void Update() {
        image.fillAmount = FULL * (player.blockingPoints / player.statSet.maxBlockingPoints);
        if (player.blockingPoints < player.statSet.maxBlockingPoints && !shieldScript.blocking)
            image.color = Color.red;
        else
            image.color = Color.yellow;
    }
}
