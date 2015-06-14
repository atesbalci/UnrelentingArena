using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public enum SkillType {
    None, Fireball, Blink, Meteor, Overcharge, Orb, Charge, Boomerang, Mine
};

public abstract class Skill {
    private float _range;
    public virtual float range { get { return _range; } set { _range = value; } }
    public Player player { get; set; }
    public Vector3 targetPosition { get; set; }
    public int level { get; set; }
    public float damage { get; set; }
    public GameObject gameObject { get; set; }
    public bool dead { get; set; }

    protected NetworkIdentity netId;

    public Skill() {
        range = 10;
        damage = 20;
        level = 1;
        dead = false;
    }

    public virtual void Start(GameObject gameObject) {
        this.gameObject = gameObject;
        netId = gameObject.GetComponent<NetworkIdentity>();
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

    public virtual void UpdateEnd() {
        Network.Destroy(gameObject);
    }
}
