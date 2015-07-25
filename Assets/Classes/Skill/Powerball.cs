using UnityEngine;
using System.Collections;

public class Powerball : SkillShot {
    private float time;
    private ParticleSystem[] particles;

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        particles = gameObject.GetComponentsInChildren<ParticleSystem>();
        particles[0].startColor = player.color;
        particles[0].Stop();
        particles[0].Play();
        time = 2;
    }

    public override void Update() {
        base.Update();
        if (maxRange)
            dead = true;
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        player.Damage(damage, this.player);
        Vector3 direction = gameObject.transform.rotation * Vector3.forward;
        collider.gameObject.GetComponent<PlayerScript>().Knockback(direction, 10, 30);
        dead = true;
    }

    public override void UpdateEnd() {
        foreach (ParticleSystem ps in particles)
            ps.Stop();
        if(time <= 0)
            Network.Destroy(gameObject);
        time -= Time.deltaTime;
    }
}
