using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {
    public Fireball(Player player)
        : base(player) {
        prefab = "Fireball";
    }

    public override void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        player.health -= 20;
        MonoBehaviour.Destroy(gameObject);
    }
}
