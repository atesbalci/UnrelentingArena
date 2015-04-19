using UnityEngine;
using System.Collections;

public enum SkillType {
    None, Fireball, Blink, Meteor
};

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

    public virtual void Start(GameObject gameObject) {
    }

    public virtual void Update(GameObject gameObject) {
    }

    public virtual void CollisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
    }

    public virtual void CollisionWithSelf(GameObject gameObject, Collider collider) {
    }

    public virtual void CollisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
    }

    public virtual void CollisionWithOtherObject(GameObject gameObject, Collider collider) {
    }

    public virtual void SerializeNetworkView(GameObject gameObject, BitStream stream, NetworkMessageInfo info) {
    }

    public void Destroy(GameObject gameObject) {
        Network.Destroy(gameObject);
    }
}
