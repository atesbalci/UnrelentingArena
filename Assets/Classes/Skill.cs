using UnityEngine;
using System.Collections;

public abstract class Skill {
    private float _range;
    public float range { get { return _range; } set { _range = value; remainingDistance = value; } }
    public string prefab { get; set; }
    public Player player { get; set; }

    protected float remainingDistance;

    public Skill(Player player) {
        range = 10;
        prefab = "";
        this.player = player;
    }

    public virtual void update(GameObject gameObject, float delta, Vector3 targetPosition) {
    }

    public virtual void collisionWithPlayer(GameObject gameObject, Collider collider, Player player) {
    }

    public virtual void collisionWithSelf(GameObject gameObject, Collider collider) {
    }

    public virtual void collisionWithSkill(GameObject gameObject, Collider collider, Skill skill) {
    }

    public virtual void collisionWithOtherObject(GameObject gameObject, Collider collider) {
    }
}
