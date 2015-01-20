using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public Skill skill { get; set; }

    void Start() {

    }

    void Update() {
        if (skill != null)
            skill.update(gameObject, Time.deltaTime);
    }

    public void OnCollisionEnter(Collision collision) {
        if (skill != null) {
            SkillScript skillScript = collision.gameObject.GetComponent<SkillScript>();
            if (skillScript != null)
                skill.collisionWithSkill(gameObject, collision, skillScript.skill);
            else
                skill.collisionWithOtherObject(gameObject, collision);
        }
    }
}
