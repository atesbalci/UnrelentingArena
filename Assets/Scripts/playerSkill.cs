using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    private ControlScript controlScript;
    private Player player;

    void Start() {
        controlScript = GetComponent<ControlScript>();
        player = GetComponent<PlayerScript>().player;
    }

    void Update() {
        Skill skill = null;

        if (GUIUtility.hotControl == 0) {
            if (controlScript.spell1) {
                skill = player.skillSet.castFireball(player);
            } else if (controlScript.spell2) {
                skill = player.skillSet.castBlink(player);
            }
        }

        if (skill != null && player.getChannel() == null) {
            Plane playerPlane = new Plane(Vector3.up, transform.position);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            float hitdist = 0.0f;

            if (playerPlane.Raycast(ray, out hitdist)) {
                Vector3 targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = targetRotation;
                skill.targetPosition = targetPoint;
                player.addBuff(new Channel(player, skill, new Vector3(transform.position.x, 1, transform.position.z), targetRotation));
            }
        }
    }
}