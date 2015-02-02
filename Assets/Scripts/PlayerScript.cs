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

    void OnGUI() {
        Vector2 targetPos;
        targetPos = Camera.main.WorldToScreenPoint(transform.position);
        targetPos.y += 70;
        Texture health = Resources.Load("UI-Elements/healthabove") as Texture;

        int width = 150;
        int height = 40;

        GUI.Box(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y, width, height), "");
        GUI.DrawTexture(new Rect(targetPos.x - width / 2 + 5, Screen.height - targetPos.y, (player.health / player.maxHealth) * (width - 10), height), health, ScaleMode.ScaleToFit, true, 10.0F);
        GUIStyle style = GUIStyle.none;
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.TextField(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y, width, height), "<color=#FFFFFF>" + (player.health + "/" + player.maxHealth) + "</color>", style);
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
