using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {

    public Fireball() {
    }

    public override void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        if (Network.isServer) {
            player.damage(damage);
            destroy(gameObject);
        }
    }
}
