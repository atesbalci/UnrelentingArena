using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Meteor : Skill {
    private enum MeteorState {
        Predamage, Damage, Postdamage
    }

    private GameObject meteor;
    private ParticleSystem explosion;
    private Image radius;
    private MeteorState state;
    private float animation;

    public Meteor()
        : base() {
        state = MeteorState.Predamage;
        animation = 6;
    }

    public override void Start(GameObject gameObject) {
        base.Start(gameObject);
        Color color = player.color;
        gameObject.transform.position = targetPosition;
        gameObject.transform.rotation = Quaternion.identity;
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        meteor = children[2].gameObject;
        meteor.GetComponentInChildren<ParticleSystem>().startColor = color;
        explosion = children[1].GetComponent<ParticleSystem>();
        meteor.transform.Translate(-100, 0, 0);
        explosion.GetComponent<ParticleSystem>().startColor = color;
        explosion.gameObject.SetActive(false);
        animation = explosion.duration + explosion.startLifetime;
        radius = gameObject.GetComponentInChildren<Image>();
        radius.color = color;
    }

    public override void Update() {
        base.Update();
        if (state == MeteorState.Predamage) {
            meteor.transform.Translate(20 * Time.deltaTime, 0, 0);
            if (meteor.transform.position.y <= 0)
                state = MeteorState.Damage;
        } else if (state == MeteorState.Damage) {
            radius.color = Color.clear;
            meteor.SetActive(false);
            explosion.gameObject.SetActive(true);
            state = MeteorState.Postdamage;
            explosion.Stop();
        } else {
            if (animation <= 0)
                Network.Destroy(gameObject);
            animation -= Time.deltaTime;
        }
    }

    public override void CollisionWithPlayer(Collider collider, Player player) {
        if (state == MeteorState.Damage) {
            player.Damage(damage, this.player);
            Vector3 direction = collider.gameObject.transform.position - gameObject.transform.position;
            direction.y = 0;
            collider.gameObject.GetComponent<PlayerScript>().Knockback(direction, 10, 30);
        }
    }
}
