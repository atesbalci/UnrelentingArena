using UnityEngine;
using System.Collections;

public abstract class SkillShot : Skill {
    public float speed { get; set; }
    public override float range { get { return base.range; } set { base.range = value; remainingDistance = value; } }
    public float remainingDistance { get; set; }

    public SkillShot() {
        speed = 16;
    }

    public override void update(GameObject gameObject) {
        base.update(gameObject);
        float travel = speed * Time.deltaTime;
        if (remainingDistance <= 0)
            destroy(gameObject);
        Vector3 prev = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        if (remainingDistance - travel >= 0) {
            gameObject.transform.Translate(0, 0, speed * Time.deltaTime);
        } else {
            gameObject.transform.Translate(0, 0, remainingDistance);
        }
        gameObject.transform.position = Vector3.Lerp(prev, gameObject.transform.position, 1);
        remainingDistance -= travel;
    }

    public override void collisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
        destroy(gameObject);
    }

    public override void serializeNetworkView(GameObject gameObject, BitStream stream, NetworkMessageInfo info) {
        float rem = 0;
        if (stream.isWriting) {
            rem = remainingDistance;
            stream.Serialize(ref rem);
        } else {
            stream.Serialize(ref rem);
            remainingDistance = rem;
        }
    }
}
