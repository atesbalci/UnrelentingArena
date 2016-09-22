using UnityEngine;
using System.Collections;

public class PulseScript : MonoBehaviour {
    public NetworkView view;
    public ParticleSystem particles;

    public Player player { get; set; }

    private const float MAX_DURATION = 1;
    private float state;
    private float damage;
    private LensFlare flare;

    void Start() {
        player = GameManager.instance.playerList[view.owner].currentPlayer;
        transform.eulerAngles = new Vector3(90, transform.eulerAngles.y, transform.eulerAngles.z);
        transform.position = new Vector3(transform.position.x, 1, transform.position.z);
        state = MAX_DURATION;
        damage = 20;
        particles.startColor = player.color;
        flare = GetComponentInChildren<LensFlare>();
        flare.color = player.color;
        particles.startRotation = transform.eulerAngles.y * Mathf.Deg2Rad;
        particles.gameObject.SetActive(true);
        particles.Stop();
    }

    void Update() {
        float scale = 3 - state / MAX_DURATION;
        transform.localScale = new Vector3(scale, scale, 1);
        flare.brightness -= Time.deltaTime / 2;
        state -= Time.deltaTime;
        if (Network.isServer && state <= 0)
            Network.Destroy(gameObject);
    }

    void OnTriggerStay(Collider col) {
        if (Network.isServer && state >= MAX_DURATION) {
            if (col.gameObject == player.gameObject)
                return;
            PlayerScript ps = col.GetComponent<PlayerScript>();
            if (ps != null) {
                ps.player.Damage(damage, player);
                ps.Knockback(ps.transform.position - new Vector3(transform.position.x, 0, transform.position.z), 10, 10);
            }
        }
    }
}
