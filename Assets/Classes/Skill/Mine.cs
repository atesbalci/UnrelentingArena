using UnityEngine;
using System.Collections;

public class Mine : TargetSkill {
    private const float FADE_DURATION = 1;

    private enum MineState {
        Ready = 0, Contact = 1, Explosion = 2, Post = 3
    }
    private MineState _state;
    private MineState state {
        get {
            return _state;
        }
        set {
            _state = value;
            if (state == MineState.Explosion) {
                gameObject.GetComponent<SphereCollider>().radius = 2.5f;
                particles.Play();
            }
        }
    }
    private ParticleSystem particles;
    private Material material;
    private float postRem;

    public Mine()
        : base() {
        state = MineState.Ready;
        postRem = FADE_DURATION;
        type = SkillType.Mine;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        gameObject.transform.position = targetPosition;
        particles = gameObject.GetComponentInChildren<ParticleSystem>();
        material = gameObject.GetComponentInChildren<Renderer>().material;
        material.SetColor("_EmissionColor", player.color);
        particles.Stop();
        particles.startColor = player.color;
    }

    public override void Update() {
        base.Update();
        if (state == MineState.Contact || state == MineState.Explosion)
            state++;
        else if (state == MineState.Post) {
            material.color = new Color(material.color.r, material.color.g, material.color.b, postRem / FADE_DURATION);
            if (postRem <= 0)
                dead = true;
            postRem -= Time.deltaTime;
        }
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        if (state == MineState.Ready) {
            state = MineState.Contact;
        } else if (state == MineState.Explosion) {
            player.Damage(preset.damage, this.player);
            collider.GetComponent<PlayerScript>().Buff(BuffType.Stun, 3);
        }
    }
}
