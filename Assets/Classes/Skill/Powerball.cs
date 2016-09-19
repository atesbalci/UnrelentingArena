using UnityEngine;
using System.Collections;

public class Powerball : SkillScript {
    private float SPEED = 30;

    private float time;
    private ParticleSystem[] particles;
    private Vector3 startPos;

    public Powerball()
        : base() {
        skillType = SkillType.Powerball;
    }

    public override void Start() {
        base.Start();
        Material mat = GetComponent<TrailRenderer>().material;
        mat.SetColor("_TintColor", player.color);
        startPos = transform.position;
    }

    public override void Update() {
        base.Update();
        canCollide = true;
        if (Vector3.Distance(startPos, transform.position) < preset.range)
            transform.position = transform.position + new Vector3(targetPosition.x - startPos.x, 0, targetPosition.z - startPos.z).normalized * SPEED * Time.deltaTime;
        else if (Network.isServer)
            Network.Destroy(gameObject);
    }

    public override void OnTriggerStay(Collider collider) {
        base.OnTriggerStay(collider);
        if (!canCollide || !Network.isServer)
            return;
        PlayerScript ps = collider.gameObject.GetComponent<PlayerScript>();
        if (ps == null || ps.player == this.player)
            return;
        ps.player.Damage(preset.damage, this.player);
        Vector3 direction = gameObject.transform.rotation * Vector3.forward;
        ps.Knockback(direction, preset.knockbackDistance, preset.knockbackSpeed);
        Network.Destroy(gameObject);
    }
}
