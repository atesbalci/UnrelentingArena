using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class PlayerScript : NetworkBehaviour {
    public Player player { get; set; }
    public SkinnedMeshRenderer bodyRenderer;

    private NetworkIdentity identity;

    [SyncVar]
    private float health;

    public PlayerScript() {
        player = new Player();
    }

    public void Initialize() {
        bodyRenderer.materials[0].SetColor("_EmissionColor", player.color * Mathf.LinearToGammaSpace(4f));
        foreach (Light light in GetComponentsInChildren<Light>())
            light.color = player.color;
        GetComponentInChildren<LensFlare>().color = player.color;
        GameObject shield = GetComponent<ShieldScript>().shield;
        foreach (MeshRenderer mr in shield.GetComponentsInChildren<MeshRenderer>()) {
            mr.material.SetColor("_EmissionColor", player.color);
        }
        shield.SetActive(false);
    }

    void Start() {
        player.Start(gameObject);
        health = player.health;
        identity = GetComponent<NetworkIdentity>();
    }

    void Update() {
        player.Update();
        if (identity.isServer) {
            health = player.health;
            if (player.health <= 0 && !player.dead) {
                player.dead = true;
                Die();
            }
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            LeaveFadingImage();
        }
        player.health = health;
    }

    public FaderScript LeaveFadingImage() {
        GameObject obj = Instantiate(bodyRenderer.gameObject.transform.parent.gameObject, transform.position, transform.rotation) as GameObject;
        FaderScript result = obj.AddComponent<FaderScript>();
        result.fadeSpeed = 1;
        return result;
    }

    [Server]
    public void Die() {
        player.Die(gameObject);
    }

    public void Buff(BuffType buff, int duration) {
        CmdBuff((int)buff, duration);
    }

    public void Knockback(Vector3 direction, float distance, float speed) {
        CmdKnockback(direction, distance, speed);
    }

    [Command]
    public void CmdKnockback(Vector3 direction, float distance, float speed) {
        player.AddBuff(new Knockback(player, gameObject, direction, distance, speed));
    }

    [Command]
    public void CmdBuff(int buff, int duration) {
        if ((BuffType)buff == BuffType.Stun) {
            player.AddBuff(new Stun(player, duration));
        }
    }
}
