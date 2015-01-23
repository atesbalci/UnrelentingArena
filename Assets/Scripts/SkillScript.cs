using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public Skill skill { get; set; }

    void Start() {

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
        //Skill s = null;
        //if (stream.isWriting) {
        //    s = skill;
        //    stream.Serialize(ref s);
        //} else {
        //    stream.Serialize(ref s);
        //    skill = s;
        //}
        if (skill != null)
            skill.serializeNetworkView(gameObject, stream, info);
    }
}
