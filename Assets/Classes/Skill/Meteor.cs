using UnityEngine;
using System.Collections;

public class Meteor : Skill {
    private enum MeteorState {
        Predamage, Damage, Postdamage
    }

    private GameObject meteor;
    private GameObject explosion;
    private MeteorState state;
    private float animation;

    public Meteor()
        : base() {
        state = MeteorState.Predamage;
        animation = 2;
    }

    public override void Start(GameObject gameObject) {
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        meteor = children[1].gameObject;
        explosion = children[2].gameObject;
        meteor.transform.Translate(-100, 0, 0);
        explosion.GetComponent<ParticleSystem>().GetComponent<Renderer>().material.SetColor("_EmissionColor", player.color);
        explosion.SetActive(false);
    }

    public override void Update(GameObject gameObject) {
        base.Update(gameObject);
        if (state == MeteorState.Predamage) {
            meteor.transform.Translate(100 * Time.deltaTime, 0, 0);
            if (meteor.transform.position.y <= 0)
                state = MeteorState.Damage;
        } else if (state == MeteorState.Damage) {
            meteor.SetActive(false);
            explosion.SetActive(true);
            state = MeteorState.Postdamage;
        } else {
            if (animation <= 0)
                Network.Destroy(gameObject);
            animation -= Time.deltaTime;
        }
    }

    public override void CollisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        if (state == MeteorState.Damage) {
            player.Damage(damage, this.player);
            collider.gameObject.GetComponent<PlayerScript>().Knockback(collider.gameObject.transform.position - gameObject.transform.position, 10, 30);
        }
    }
}
