using UnityEngine;
using System.Collections;

public class SwapBeam : SkillScript {
    private float SPEED = 30;

    private float time;
    private ParticleSystem[] particles;
    private Vector3 startPos;
    private LineRenderer line;

    public SwapBeam()
        : base() {
        skillType = SkillType.SwapBeam;
    }

    public override void Start() {
        base.Start();
        line = GetComponent<LineRenderer>();
        ParticleSystem particles = GetComponent<ParticleSystem>();
        particles.startColor = player.color;
        particles.Stop();
        particles.Play();
        line.material.SetColor("_TintColor", player.color);
        GetComponent<MeshRenderer>().material.SetColor("_MainColor", player.color);
        startPos = transform.position;
    }

    public override void Update() {
        base.Update();
        canCollide = true;
        if (Vector3.Distance(startPos, transform.position) < preset.range)
            transform.position = transform.position + new Vector3(targetPosition.x - startPos.x, 0, targetPosition.z - startPos.z).normalized * SPEED * Time.deltaTime;
        else if (Network.isServer)
            Network.Destroy(gameObject);
        line.SetPosition(0, player.gameObject.transform.position);
        line.SetPosition(1, transform.position);
    }

    public override void OnTriggerStay(Collider collider) {
        base.OnTriggerStay(collider);
        if (!canCollide || !Network.isServer)
            return;
        PlayerScript ps = collider.gameObject.GetComponent<PlayerScript>();
        if (ps == null || ps.player == this.player)
            return;
        ps.player.Damage(preset.damage, this.player);
        Vector3 tmp = player.gameObject.transform.position;
        Vector3 tmp2 = ps.transform.position;
        player.gameObject.GetComponent<PlayerScript>().Move(tmp2);
        ps.Move(tmp);
        Network.Destroy(gameObject);
    }
}
