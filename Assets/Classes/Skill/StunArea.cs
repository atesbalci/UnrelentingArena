using UnityEngine;
using System.Collections;

public class StunArea : SkillScript {
    private ParticleSystem particles;

	public override void Start () {
        base.Start();
        CollideOnce();
        transform.position = targetPosition;
        particles = GetComponent<ParticleSystem>();
        particles.startColor = player.color;
        particles.Stop();
        particles.Play();
        particles.Stop();
	}

    public override void Update() {
        base.Update();
        if (!particles.IsAlive() && Network.isServer)
            Network.Destroy(gameObject);
    }

    public override void OnTriggerStay(Collider col) {
        base.OnTriggerStay(col);
        if (!canCollide || !Network.isServer)
            return;
        PlayerScript ps = col.gameObject.GetComponent<PlayerScript>();
        if (ps == null || ps.player == this.player)
            return;
        ps.Buff(BuffType.Stun, 2);
        ps.player.Damage(preset.damage, player);
    }
}
