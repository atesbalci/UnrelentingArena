using UnityEngine;
using System.Collections;

public abstract class Skill {
    private float _range;
    public virtual float range { get { return _range; } set { _range = value; } }
    public Player player { get; set; }
    public Vector3 targetPosition { get; set; }
    public float damage { get; set; }

    public Skill() {
        range = 10;
        damage = 20;
    }

    public virtual void update(GameObject gameObject) {
    }

    public virtual void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
    }

    public virtual void collisionWithSelf(GameObject gameObject, Collider collider) {
    }

    public virtual void collisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
    }

    public virtual void collisionWithOtherObject(GameObject gameObject, Collider collider) {
    }

    public virtual void serializeNetworkView(GameObject gameObject, BitStream stream, NetworkMessageInfo info) {
        float dmg = 0;
        float rng = 0;

        if (stream.isWriting) {
            rng = range;
            dmg = damage;
            stream.Serialize(ref rng);
            stream.Serialize(ref dmg);
        } else {
            stream.Serialize(ref rng);
            stream.Serialize(ref dmg);
            damage = dmg;
            range = rng;
        }
    }

    public void destroy(GameObject gameObject) {
        MonoBehaviour.Destroy(gameObject);
    }
}
