using UnityEngine;
using System.Collections;

public enum SkillType {
    None, Fireball, Blink, Meteor, Overcharge, Orb, Charge
};

public abstract class Skill {
    private float _range;
    public virtual float range { get { return _range; } set { _range = value; } }
    public Player player { get; set; }
    public Vector3 targetPosition { get; set; }
    public int level { get; set; }
    public float damage { get; set; }
    public GameObject gameObject { get; set; }

    public Skill() {
        range = 10;
        damage = 20;
        level = 1;
    }

    public virtual void Start(GameObject gameObject) {
        this.gameObject = gameObject;
    }

    public virtual void Update() {
    }

    public virtual void CollisionWithPlayer(Collider collider, Player player) {
    }

    public virtual void CollisionWithSelf(Collider collider) {
    }

    public virtual void CollisionWithSkill(Collider collider, Skill skill) {
    }

    public virtual void CollisionWithOtherObject(Collider collider) {
    }

    public virtual void SerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
    }
}
