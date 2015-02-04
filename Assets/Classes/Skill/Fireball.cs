using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {

    public Fireball() {
    }

    public override void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        player.damage(damage);
        Vector3 direction = gameObject.transform.rotation * Vector3.forward;
        collider.gameObject.GetComponent<PlayerScript>().knockback(direction, 10, 30);
        destroy(gameObject);
    }
}
