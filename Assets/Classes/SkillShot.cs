using UnityEngine;
using System.Collections;

public abstract class SkillShot : Skill {
    public float speed { get; set; }

    public SkillShot(Player player)
        : base(player) {
        speed = 16;
    }

    public override void update(GameObject gameObject, float delta, Vector3 targetPosition) {
        float travel = speed * delta;
        if (remainingDistance <= 0)
            MonoBehaviour.Destroy(gameObject);
        if (remainingDistance - travel >= 0) {
            gameObject.transform.Translate(0, 0, speed * delta);
        } else {
            gameObject.transform.Translate(0, 0, remainingDistance);
        }
        remainingDistance -= travel;
    }

    public override void collisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
        MonoBehaviour.Destroy(gameObject);
    }
}
