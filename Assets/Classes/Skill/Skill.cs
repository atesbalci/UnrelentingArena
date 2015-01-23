using UnityEngine;
using System.Collections;

public abstract class Skill {
    private float _range;
    public virtual float range { get { return _range; } set { _range = value; } }
    public string prefab { get; set; }
    public Player player { get; set; }
    public Vector3 targetPosition { get; set; }
    public float channelTime { get; set; }
    public float recoilTime { get; set; }

    public Skill(Player player) {
        range = 10;
        prefab = "";
        this.player = player;
        channelTime = 0.5f;
        recoilTime = 0.5f;
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
}
