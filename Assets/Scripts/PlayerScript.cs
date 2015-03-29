using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }
    public Color color { get; set; }
    public SkinnedMeshRenderer bodyRenderer;

    private NetworkView view;

    public PlayerScript() {
        player = new Player();
    }

    void Start() {
        view = GetComponent<NetworkView>();
        bodyRenderer.material.SetColor("_EmissionColor", color);
        foreach (Light light in GetComponentsInChildren<Light>())
            light.color = color;
        GetComponentInChildren<LensFlare>().color = color;
        GetComponent<ShieldScript>().shield.SetActive(false);
    }

    void Update() {
        player.Update(gameObject);
        if (Network.isServer) {
            if (player.health <= 0 && !player.dead) {
                player.dead = true;
                view.RPC("Die", RPCMode.AllBuffered);
            }
        }
    }

    [RPC]
    public void Die() {
        player.Die(gameObject);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float health = player.health;
            stream.Serialize(ref health);
        } else {
            float health = -1;
            stream.Serialize(ref health);
            player.health = health;
        }
    }

    public void Knockback(Vector3 direction, float distance, float speed) {
        view.RPC("ApplyKnockback", RPCMode.AllBuffered, direction, distance, speed);
    }

    [RPC]
    public void ApplyKnockback(Vector3 direction, float distance, float speed) {
        player.AddBuff(new Knockback(player, gameObject, direction, distance, speed));
    }
}
