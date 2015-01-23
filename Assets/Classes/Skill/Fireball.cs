using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {

    public Fireball() {
        prefab = "Fireball";
    }

    public override void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        player.health -= damage;
        destroy(gameObject);
    }
}
