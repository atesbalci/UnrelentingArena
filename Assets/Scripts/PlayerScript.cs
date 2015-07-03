using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }
    public SkinnedMeshRenderer bodyRenderer;

    public NetworkView view;

    public PlayerScript() {
        player = new Player();
    }

    public void Initialize() {
        bodyRenderer.materials[0].SetColor("_EmissionColor", player.color * Mathf.LinearToGammaSpace(4f));
        foreach (Light light in GetComponentsInChildren<Light>())
            light.color = player.color;
        GetComponentInChildren<LensFlare>().color = player.color;
        GameObject shield = GetComponent<EnergyScript>().shield;
        foreach (MeshRenderer mr in shield.GetComponentsInChildren<MeshRenderer>()) {
            mr.material.SetColor("_EmissionColor", player.color);
        }
        shield.SetActive(false);
        if(Network.isServer) {
            view.RPC("SwitchOwner", RPCMode.All, Network.AllocateViewID());
        }
    }

    [RPC]
    public void SwitchOwner(NetworkViewID id) {
        view.viewID = id;
    }

    void Start() {
        player.Start(gameObject);
    }

    void Update() {
        player.Update();
        if (Network.isServer) {
            if (player.health <= 0 && !player.dead) {
                player.dead = true;
                view.RPC("Die", RPCMode.AllBuffered);
            }
        }
        if (Input.GetKeyDown(KeyCode.G)) {
            LeaveFadingImage();
        }
    }

    public FaderScript LeaveFadingImage() {
        GameObject obj = Instantiate(bodyRenderer.gameObject.transform.parent.gameObject, transform.position, transform.rotation) as GameObject;
        FaderScript result = obj.AddComponent<FaderScript>();
        result.fadeSpeed = 1;
        return result;
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

    public void Buff(BuffType buff, int duration) {
        view.RPC("ApplyBuff", RPCMode.All, (int)buff, duration);
    }

    public void Knockback(Vector3 direction, float distance, float speed) {
        view.RPC("ApplyKnockback", RPCMode.AllBuffered, direction, distance, speed);
    }

    [RPC]
    public void ApplyKnockback(Vector3 direction, float distance, float speed) {
        player.AddBuff(new Knockback(player, gameObject, direction, distance, speed));
    }

    [RPC]
    public void ApplyBuff(int buff, int duration) {
        if ((BuffType)buff == BuffType.Stun) {
            player.AddBuff(new Stun(player, duration));
        }
    }
}
