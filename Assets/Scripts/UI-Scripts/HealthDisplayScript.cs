using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplayScript : MonoBehaviour {
    private Player player;

    private Color fullHealth;
    private Color lowHealth;

    void Start() {
        player = GetComponentInParent<PlayerScript>().player;
        fullHealth = Color.green;
        lowHealth = Color.red;
    }

    void Update() {
        GetComponent<Image>().fillAmount = player.health / player.maxHealth;
        GetComponent<Image>().color = Color.Lerp(lowHealth, fullHealth, player.health / player.maxHealth);
    }
}
