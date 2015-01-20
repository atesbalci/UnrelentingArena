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
        player.update(gameObject, Time.deltaTime);
    }
}
