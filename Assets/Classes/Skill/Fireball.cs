using UnityEngine;
using System.Collections;

public class Fireball : SkillShot {
    public float damage { get; set; }

    public Fireball(Player player)
        : base(player) {
        prefab = "Fireball";
        damage = 20;
    }

    public override void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
        player.health -= damage;
        MonoBehaviour.Destroy(gameObject);
    }
}
