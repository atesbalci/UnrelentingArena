using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyUIScript : MonoBehaviour {
    public Image image;

    private Player player;
    private EnergyScript shieldScript;
    private const float FULL = 0.235f;

    void Start() {
        player = GetComponentInParent<PlayerScript>().player;
        shieldScript = GetComponentInParent<EnergyScript>();
    }

    void Update() {
        image.fillAmount = FULL * (player.energyPoints / player.statSet.maxEnergyPoints);
        if (player.energyPoints < player.statSet.maxEnergyPoints && !shieldScript.blocking)
            image.color = Color.red;
        else
            image.color = Color.yellow;
    }
}
