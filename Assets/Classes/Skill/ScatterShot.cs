using UnityEngine;
using System.Collections;

public class ScatterShot : MonoBehaviour {
    private Scatter scatter;

    void Start() {
        scatter = GetComponentInParent<Scatter>();
    }

    void OnTriggerEnter(Collider col) {
        if (!Network.isServer)
            return;
        PlayerScript ps = col.GetComponent<PlayerScript>();
        if (ps != null && ps.player != scatter.player) {
            ps.player.Damage(scatter.preset.damage, scatter.player);
            Vector3 direction = gameObject.transform.rotation * Vector3.forward;
            ps.Knockback(direction, scatter.preset.knockbackDistance, scatter.preset.knockbackSpeed);
            Network.Destroy(gameObject);
        }
    }
}
