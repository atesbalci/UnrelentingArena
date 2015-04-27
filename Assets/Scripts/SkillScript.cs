using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public SkillType skillType;
    public NetworkView view;
    public Skill skill { get; set; }

    void Start() {
        Initialize();
        if (skill != null)
            skill.Start(gameObject);
    }

    public void Initialize() {
        if (skill == null) {
            switch (skillType) {
                case SkillType.Fireball:
                    skill = new Fireball();
                    break;
                case SkillType.Blink:
                    skill = new Blink();
                    break;
                case SkillType.Meteor:
                    skill = new Meteor();
                    break;
                case SkillType.Overcharge:
                    skill = new Overcharge();
                    break;
                case SkillType.Orb:

                    break;
                default:
                    skill = null;
                    break;
            }
        }
    }

    void Update() {
        if (skill != null)
            skill.Update(gameObject);
    }

    void OnTriggerStay(Collider collider) {
        if (skill != null && Network.isServer) {
            SkillScript skillScript = collider.gameObject.GetComponent<SkillScript>();
            PlayerScript playerScript = collider.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null) {
                if (skill.player != playerScript.player)
                    skill.CollisionWithPlayer(gameObject, collider, playerScript.player);
                else
                    skill.CollisionWithSelf(gameObject, collider);
            } else if (skillScript != null)
                skill.CollisionWithSkill(gameObject, collider, skillScript.skill);
            else
                skill.CollisionWithOtherObject(gameObject, collider);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (skill != null)
            skill.SerializeNetworkView(gameObject, stream, info);
    }
}
