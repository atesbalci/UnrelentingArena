using UnityEngine;
using System.Collections;

public class SwingHitbox : MonoBehaviour {
    public int detectCollision { get; set; }

    public Player player { get; set; }

    void Awake() {
        PlayerScript ps = GetComponentInParent<PlayerScript>();
        player = ps.player;
        foreach (Blade b in ps.blades) {
            b.hitbox = this;
        }
    }

    void Start() {
        detectCollision = 0;
    }

    void FixedUpdate() {
        if (detectCollision > 0)
            detectCollision--;
    }

    void OnTriggerStay(Collider col) {
        if (!Network.isServer)
            return;
        if (detectCollision > 0) {
            PlayerScript p = col.GetComponent<PlayerScript>();
            if (p != null)
                if (p.player != player) {
                    p.player.Damage(10, player);
                }
        }
    }
}
