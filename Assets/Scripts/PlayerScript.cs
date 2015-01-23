using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }

    public PlayerScript() {
        player = new Player();
    }

    void Start() {
    
    }

    void Update() {
        player.update(gameObject);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        float health = 100;
        if (stream.isWriting) {
            health = player.health;
            stream.Serialize(ref health);
        } else {
            stream.Serialize(ref health);
            player.health = health;
        }
    }
}
