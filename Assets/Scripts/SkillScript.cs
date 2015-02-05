using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public SkillType skillType;
    public Skill skill { get; set; }

    void Start() {
        initialize();
    }

    public void initialize() {
        if (skill == null) {
            switch (skillType) {
                case SkillType.fireball:
                    skill = new Fireball();
                    break;
                case SkillType.blink:
                    skill = new Blink();
                    break;
                default:
                    skill = null;
                    break;
            }
        }
    }

    void Update() {
        if (skill != null)
            skill.update(gameObject);
    }

    void OnTriggerEnter(Collider collider) {
        if (skill != null && Network.isServer) {
            SkillScript skillScript = collider.gameObject.GetComponent<SkillScript>();
            PlayerScript playerScript = collider.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null) {
                if (skill.player != playerScript.player)
                    skill.collisionWithPlayer(gameObject, collider, playerScript.player);
                else
                    skill.collisionWithSelf(gameObject, collider);
            } else if (skillScript != null)
                skill.collisionWithSkill(gameObject, collider, skillScript.skill);
            else
                skill.collisionWithOtherObject(gameObject, collider);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (skill != null)
            skill.serializeNetworkView(gameObject, stream, info);
    }
}
