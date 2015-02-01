using UnityEngine;
using System.Collections;

public abstract class Skill {
    private float _range;
    public virtual float range { get { return _range; } set { _range = value; } }
    public Player player { get; set; }
    public Vector3 targetPosition { get; set; }
    public int level { get; set; }
    public float damage { get; set; }

    public Skill() {
        range = 10;
        damage = 20;
        level = 1;
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
    }

    public void destroy(GameObject gameObject) {
        Network.Destroy(gameObject);
    }
}
