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
        float ratio = player.health / player.maxHealth;
        health.fillAmount = ratio;
        health.color = Color.Lerp(lowHealth, fullHealth, ratio);
        if (ratio - previous > 0.001 || ratio - previous < 0.001) {
            previous = Mathf.Lerp(previous, ratio, 0.1f);
        }
        lostHealth.fillAmount = previous;
    }
}
