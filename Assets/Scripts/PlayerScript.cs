using UnityEngine;
using System.Collections;

public class PlayerScript : MonoBehaviour {
    public Player player { get; set; }
    public GameObject deathPrefab;

    public NetworkView view;

    private SkinnedMeshRenderer[] bodyRenderers;
    private FuryEffect furyEffect;
    private MomentumScript momentum;

    public PlayerScript() {
        player = new Player();
    }

    public void Initialize() {
        bodyRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
        furyEffect = GetComponentInChildren<FuryEffect>();
        furyEffect.gameObject.SetActive(false);
        momentum = GetComponentInChildren<MomentumScript>();
        foreach (SkinnedMeshRenderer r in bodyRenderers)
            r.materials[0].SetColor("_EmissionColor", player.color * Mathf.LinearToGammaSpace(4f));
        foreach (Light light in GetComponentsInChildren<Light>())
            light.color = player.color;
        if (Network.isServer) {
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
        furyEffect.gameObject.SetActive(player.modifier == ComboModifier.Fury);
        momentum.GainMomentum(player.modifier == ComboModifier.Momentum);
    }

    public FaderScript LeaveFadingImage() {
        GameObject obj = Instantiate(bodyRenderers[0].gameObject.transform.parent.gameObject, transform.position, transform.rotation) as GameObject;
        FaderScript result = obj.AddComponent<FaderScript>();
        result.fadeSpeed = 1;
        return result;
    }

    [RPC]
    public void Die() {
        player.Die();
        SpawnDeathAnimation();
        Destroy(gameObject);
    }

    public void SpawnDeathAnimation() {
        GameObject death = (GameObject)Instantiate(deathPrefab, transform.position, transform.rotation);
        ParticleSystem ps = death.GetComponentInChildren<ParticleSystem>();
        ps.startColor = player.color;
        ps.Stop();
        ps.Play();
        death.GetComponentInChildren<LensFlare>().color = player.color;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            float health = player.health;
            int modifier = (int)player.modifier;
            stream.Serialize(ref health);
            stream.Serialize(ref modifier);
        } else {
            float health = -1;
            int modifier = 0;
            stream.Serialize(ref health);
            stream.Serialize(ref modifier);
            player.health = health;
            player.modifier = (ComboModifier)modifier;
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
