using UnityEngine;
using System.Collections;

public class Implosion : SkillScript {
    private ParticleSystem particleSystem;

    public Implosion()
        : base() {
        skillType = SkillType.Implosion;
    }

    public override void Start() {
        base.Start();
        particleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        particleSystem.startColor = player.color;
        particleSystem.gameObject.SetActive(false);
        particleSystem.gameObject.SetActive(true);
        particleSystem.Stop();
        CollideOnce();
    }

    public override void Update() {
        if (!particleSystem.IsAlive())
            Destroy(gameObject);
    }

    public override void OnTriggerStay(Collider collider) {
        base.OnTriggerStay(collider);
        if (!canCollide || !Network.isServer)
            return;
        PlayerScript ps = collider.gameObject.GetComponent<PlayerScript>();
        if (ps == null || ps.player == this.player)
            return;
        player.Damage(preset.damage, player);
        Vector3 direction = -(collider.gameObject.transform.position - gameObject.transform.position);
        direction.y = 0;
        collider.GetComponent<PlayerScript>().Knockback(direction, preset.knockbackDistance, preset.knockbackSpeed);
    }
}
