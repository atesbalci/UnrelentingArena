using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class HealthDisplayScript : MonoBehaviour {
    public Image lostHealth;

    private Image health;
    private Player player;

    private Color fullHealth;
    private Color lowHealth;
    private float previous;

    void Start() {
        player = GetComponentInParent<PlayerScript>().player;
        health = GetComponent<Image>();
        fullHealth = Color.green;
        lowHealth = Color.red;
        previous = 1.0f;
    }

    void Update() {
        float ratio = player.health / player.statSet.maxHealth;
        health.fillAmount = ratio;
        health.color = Color.Lerp(lowHealth, fullHealth, ratio);
        if (previous - ratio >= 0.001f) {
            previous -= Time.deltaTime * 0.1f;
            if (previous < ratio)
                previous = ratio;
            lostHealth.fillAmount = previous;
        }
    }
}
