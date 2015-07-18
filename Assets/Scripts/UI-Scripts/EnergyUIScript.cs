using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnergyUIScript : MonoBehaviour {
    public Image image;

    private Player player;
    private EnergyScript energyScript;
    private const float FULL = 0.235f;

    void Start() {
        player = GetComponentInParent<PlayerScript>().player;
        energyScript = GetComponentInParent<EnergyScript>();
    }

    void Update() {
        image.fillAmount = FULL * (player.energyPoints / player.statSet.maxEnergyPoints);
        if (player.energyPoints < player.statSet.maxEnergyPoints)
            image.color = Color.red;
        else
            image.color = Color.yellow;
    }
}
