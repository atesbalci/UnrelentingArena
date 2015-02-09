using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }

    public PlayerScript() {
        player = new Player();
    }

    void Start() {
        health = Resources.Load("UI-Elements/healthabove") as Texture;
        back = Resources.Load("UI-Elements/castbar-back") as Texture;
    }

    void Update() {
        player.Update(gameObject);
    }

    private Texture health;
    private Texture back;

    void OnGUI() {
        Vector2 targetPos = Camera.main.WorldToScreenPoint(transform.position);
        targetPos.y += 70;

        int width = 200;
        int height = 50;

        GUIStyle style = new GUIStyle();
        style.richText = true;
        style.alignment = TextAnchor.MiddleCenter;
        GUI.Label(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y - 30, width, height), "<color=#FFFFFF>" + player.name + "</color>", style);
        GUI.DrawTexture(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y, width, height), back, ScaleMode.ScaleAndCrop);
        GUI.DrawTexture(new Rect(targetPos.x - width / 2 + 18, Screen.height - targetPos.y + 17, (player.health / player.maxHealth) * (width - 36), height - 34), health, ScaleMode.ScaleAndCrop);
        GUI.Label(new Rect(targetPos.x - width / 2, Screen.height - targetPos.y, width, height), "<color=#FFFFFF>" + (player.health + "/" + player.maxHealth) + "</color>", style);
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
        networkView.RPC("ApplyKnockback", RPCMode.AllBuffered, direction, distance, speed);
    }

    [RPC]
    public void ApplyKnockback(Vector3 direction, float distance, float speed) {
        player.AddBuff(new Knockback(player, gameObject, direction, distance, speed));
    }
}
