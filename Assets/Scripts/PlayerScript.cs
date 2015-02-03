using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }

    private Texture health;
    private Texture back;

    public PlayerScript() {
        player = new Player();
        health = Resources.Load("UI-Elements/healthabove") as Texture;
        back = Resources.Load("UI-Elements/castbar-back") as Texture;
    }

    void Start() {
    }

    void Update() {
        player.update(gameObject);
    }

    void OnGUI() {
        Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);
        targetPos.y += 70;

        int width = 200;
        int height = 50;

        GUI.DrawTexture(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y, width, height), back, ScaleMode.ScaleAndCrop);
        GUI.DrawTexture(new Rect(targetPos.x - width / 2 + 18, Screen.height - targetPos.y + 17, (player.health / player.maxHealth) * (width - 36), height - 34), health, ScaleMode.ScaleAndCrop);
        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y, width, height), "<color=#FFFFFF>" + (player.health + "/" + player.maxHealth) + "</color>", style);
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
