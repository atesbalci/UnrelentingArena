using UnityEngine;
using System.Collections;

public class Overcharge : Skill {
    private float remaining;
    private ParticleSystem particleSystem;

    public Overcharge()
        : base() {
        type = SkillType.Overcharge;
    }

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
        player.Damage(preset.damage, player);
        Vector3 direction = collider.gameObject.transform.position - gameObject.transform.position;
        direction.y = 0;
        collider.GetComponent<PlayerScript>().Knockback(direction, preset.knockbackDistance, preset.knockbackSpeed);
    }

    public override void UpdateEnd() {
        if (remaining <= 0)
            Network.Destroy(gameObject);
        remaining -= Time.deltaTime;
    }
}
