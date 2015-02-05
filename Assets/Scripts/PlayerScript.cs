using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }

    private Texture health;
    private Texture back;

    void Start() {
        health = Resources.Load("UI-Elements/healthabove") as Texture;
        back = Resources.Load("UI-Elements/castbar-back") as Texture;
    }

    void Update() {
        player.update(gameObject);
        if (Network.isServer)
            networkView.RPC("refreshHealth", RPCMode.AllBuffered, player.health);
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

    [RPC]
    public void refreshHealth(float health) {
        player.health = health;
    }

    public void knockback(Vector3 direction, float distance, float speed) {
        networkView.RPC("applyKnockback", RPCMode.AllBuffered, direction, distance, speed);
    }

    [RPC]
    public void applyKnockback(Vector3 direction, float distance, float speed) {
        player.addBuff(new Knockback(player, gameObject, direction, distance, speed));
    }
}
