using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public Skill skill { get; set; }
    public Vector3 targetPosition { get; set; }

    void Start() {

    }

    void Update() {
        if (skill != null)
            skill.update(gameObject, Time.deltaTime, targetPosition);
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
}
