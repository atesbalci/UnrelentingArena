using UnityEngine;
using System.Collections;

public abstract class SkillShot : Skill {
    public float speed { get; set; }
    public override float range { get { return base.range; } set { base.range = value; remainingDistance = value; } }

    protected float remainingDistance;

    public SkillShot(Player player)
        : base(player) {
        speed = 16;
    }

    public override void update(GameObject gameObject) {
        float travel = speed * Time.deltaTime;
        if (remainingDistance <= 0)
            Network.Destroy(gameObject);
        if (remainingDistance - travel >= 0) {
            gameObject.transform.Translate(0, 0, speed * Time.deltaTime);
        } else {
            gameObject.transform.Translate(0, 0, remainingDistance);
        }
        remainingDistance -= travel;
    }

    public override void collisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
        Network.Destroy(gameObject);
    }
}
