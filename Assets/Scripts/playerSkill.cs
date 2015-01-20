using UnityEngine;
using System.Collections;

public class playerSkill : MonoBehaviour {
    public KeyCode skillKey1 = KeyCode.Alpha1;
    public KeyCode skillKey2 = KeyCode.Alpha2;

    private SkillSet skillSet;

    void Start() {
        skillSet = new SkillSet();
    }

    void Update() {
        Skill skill = null;
        Player player = gameObject.GetComponent<PlayerScript>().player;

        if (GUIUtility.hotControl == 0) {
            if (Input.GetKeyDown(skillKey1)) {
                skill = skillSet.castFireball(player);
            } else if (Input.GetKeyDown(skillKey2)) {
                skill = skillSet.castBlink(player);
            }
        }

        if (skill != null) {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist)) {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = targetRotation;
                GameObject skillGameObject = Instantiate(Resources.Load(skill.prefab, typeof(GameObject)), new Vector3(transform.position.x, 1, transform.position.z), targetRotation) as GameObject;
                SkillScript skillScript = skillGameObject.AddComponent<SkillScript>();
                skillScript.skill = skill;
                skillScript.targetPosition = targetPoint;
            }
        }
    }
}