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
            Player player = null;
            foreach (GameObject go in GameObject.FindGameObjectsWithTag("Player")) {
                if (go.GetComponent<NetworkView>().owner == networkView.owner) {
                    player = go.GetComponent<PlayerScript>().player;
                    break;
                }
            }
            if (skillType == SkillType.fireball)
                skill = new Fireball();
            else if (skillType == SkillType.blink)
                skill = new Blink();

            skill.player = player;
        }
    }

    void Update() {
        if (skill != null)
            skill.update(gameObject);
    }

    void OnTriggerEnter(Collider collider) {
        if (skill != null) {
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
