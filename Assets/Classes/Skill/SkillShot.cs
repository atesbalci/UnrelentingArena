using UnityEngine;
using System.Collections;

public abstract class SkillShot : Skill {
    public float speed { get; set; }
    public override float range { get { return base.range; } set { base.range = value; remainingDistance = value; } }
    public float remainingDistance { get; set; }

    public SkillShot() {
        speed = 16;
    }

    public override void Update(GameObject gameObject) {
        base.Update(gameObject);
        float travel = speed * Time.deltaTime;
        if (Network.isServer && remainingDistance <= 0)
            Destroy(gameObject);
        Vector3 prev = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y, gameObject.transform.position.z);
        if (remainingDistance - travel <= 0) {
            travel = remainingDistance;
        }
        gameObject.transform.Translate(0, 0, travel);
        gameObject.transform.position = Vector3.Lerp(prev, gameObject.transform.position, 1);
        remainingDistance -= travel;
    }

    public override void CollisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
        Destroy(gameObject);
    }

    public override void SerializeNetworkView(GameObject gameObject, BitStream stream, NetworkMessageInfo info) {
        
    }
}
