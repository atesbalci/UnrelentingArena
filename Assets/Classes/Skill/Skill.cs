using UnityEngine;
using System.Collections;

public enum SkillType {
    None = -1, Powerball = 0, Blink = 1, StunArea = 2, Overcharge = 3, SwapBeam = 4, Charge = 5, Scatter = 6, Implosion = 7
};

public abstract class Skill {
    public Player player { get; set; }
    public Vector3 targetPosition { get; set; }
    public SkillPreset preset { get; set; }
    public GameObject gameObject { get; set; }
    public bool dead { get; set; }
    public SkillType type { get; set; }
    public ComboModifier modifier { get; set; }

    public Skill() {
        type = SkillType.None;
    }

    public virtual void Start(GameObject gameObject) {
        this.gameObject = gameObject;
        preset = player.skillSet.skills[type];
        dead = false;
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

    public virtual void UpdateEnd() {
        Network.Destroy(gameObject);
    }
}
