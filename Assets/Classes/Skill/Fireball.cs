using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {
    public override void CollisionWithPlayer(Collider collider, Player player) {
        player.Damage(damage, this.player);
        Vector3 direction = gameObject.transform.rotation * Vector3.forward;
        collider.gameObject.GetComponent<PlayerScript>().Knockback(direction, 10, 30);
        Network.Destroy(gameObject);
    }
}
