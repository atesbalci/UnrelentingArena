using UnityEngine;
using System.Collections;

public class Overcharge : Skill {
    private bool damaging, done;
    private float remaining;
    private ParticleSystem particleSystem;

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        damaging = true;
        done = false;
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        particleSystem.startColor = player.color;
        particleSystem.gameObject.SetActive(false);
        particleSystem.gameObject.SetActive(true);
    }

    public override void Update() {
        if (damaging && !done) {
            done = true;
        } else if (damaging) {
            damaging = false;
            remaining = particleSystem.startLifetime;
            particleSystem.Stop();
        } else {
            if (remaining <= 0)
                Network.Destroy(gameObject);
            remaining -= Time.deltaTime;
        }
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        if (damaging) {
            player.Damage(damage, player);
            collider.GetComponent<PlayerScript>().Knockback(collider.gameObject.transform.position - gameObject.transform.position, 10, 30);
        }
    }
}
