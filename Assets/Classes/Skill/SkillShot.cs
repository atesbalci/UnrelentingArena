using UnityEngine;
using System.Collections;

public abstract class SkillShot : Skill {
    public float speed { get; set; }
    public override float range { get { return base.range; } set { base.range = value; remainingDistance = value; } }
    public float remainingDistance { get; set; }
    
    protected bool maxRange;

    public SkillShot()
        : base() {
        speed = 16;
        maxRange = false;
    }

    public override void Update() {
        base.Update();
        float travel = speed * Time.deltaTime;
        if (Network.isServer && remainingDistance <= 0)
            maxRange = true;
        if (!maxRange) {
            Vector3 prev = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
            if (remainingDistance - travel <= 0) {
                travel = remainingDistance;
            }
            gameObject.transform.Translate(0, 0, travel);
            gameObject.transform.position = Vector3.Lerp(prev, gameObject.transform.position, 1);
            remainingDistance -= travel;
        }
    }

    public override void CollisionWithSkill(Collider collider, Skill skill) {
        dead = true;
    }
}
