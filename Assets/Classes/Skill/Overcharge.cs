using UnityEngine;
using System.Collections;

public class Overcharge : Skill {
    private float remaining;
    private ParticleSystem particleSystem;

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        particleSystem.startColor = player.color;
        particleSystem.gameObject.SetActive(false);
        particleSystem.gameObject.SetActive(true);
    }

    public override void Update() {
        remaining = particleSystem.startLifetime;
        particleSystem.Stop();
        dead = true;
        remaining = particleSystem.startLifetime;
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        Debug.Log("collide!");
        player.Damage(damage, player);
        Vector3 direction = collider.gameObject.transform.position - gameObject.transform.position;
        direction.y = 0;
        collider.GetComponent<PlayerScript>().Knockback(direction, 10, 30);
    }

    public override void UpdateEnd() {
        if (remaining <= 0)
            Network.Destroy(gameObject);
        remaining -= Time.deltaTime;
    }
}
