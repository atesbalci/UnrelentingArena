using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {

    public Fireball() {
    }

    public override void CollisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        player.Damage(damage, player);
        Vector3 direction = gameObject.transform.rotation * Vector3.forward;
        collider.gameObject.GetComponent<PlayerScript>().Knockback(direction, 10, 30);
        Destroy(gameObject);
    }
}
