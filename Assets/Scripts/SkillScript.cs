using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public NetworkView view;
    public SkillType skillType { get; set; }
    public Vector3 targetPosition { get; set; }
    public Player player { get; set; }
    public ComboModifier modifier { get; set; }
    public SkillPreset preset { get; set; }
    public bool canCollide {
        get {
            return collisionCounter >= 0;
        }
        set {
            collisionCounter = value ? int.MaxValue : -1;
        }
    }

    private int collisionCounter;

    public virtual void Start() {
        collisionCounter = -1;
        preset = player.skillSet.skills[skillType];
    }

    public virtual void Update() {
        
    }

    public virtual void FixedUpdate() {
        if (collisionCounter >= 0)
            collisionCounter--;
    }

    protected void CollideOnce() {
        collisionCounter = 1;
    }

    public virtual void OnTriggerStay(Collider col) {

    }
}
