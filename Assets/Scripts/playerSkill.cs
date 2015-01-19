using UnityEngine;
using System.Collections;

public class playerSkill : MonoBehaviour {
    public KeyCode skillKey1 = KeyCode.Alpha1;

    private SkillSet skillSet;

    void Start() {
        skillSet = new SkillSet();
    }

    void Update() {
        Skill skill = null;

        if (Input.GetKeyDown(skillKey1) && GUIUtility.hotControl == 0) {
            skill = skillSet.castFireball();
        }

        if (skill != null) {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist)) {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = targetRotation;
                GameObject skillGameObject = Instantiate(Resources.Load(skill.getPrefab(), typeof(GameObject)), transform.position, targetRotation) as GameObject;
                SkillScript skillScript = skillGameObject.AddComponent<SkillScript>();
                skillScript.setRemainingDistance(skill.getRange());
            }
        }
    }
}