using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    private ControlScript controlScript;
    private Player player;
    private Vector3 targetPoint;
    private int casting;

    void Start() {
        controlScript = GetComponent<ControlScript>();
        player = GetComponent<PlayerScript>().player;
        casting = 0;
    }

    void Update() {
        if (networkView.isMine) {
            if (GUIUtility.hotControl == 0) {
                if (controlScript.spell1) {
                    casting = 1;
                } else if (controlScript.spell2) {
                    casting = 2;
                }
            }

            if (player.toBeCast != null) {
                GameObject skillObject = Network.Instantiate(Resources.Load(player.toBeCast.skill.prefab), player.toBeCast.position, player.toBeCast.rotation, 0) as GameObject;
                networkView.RPC("initializeSkill", RPCMode.AllBuffered, skillObject.networkView.viewID, player.toBeCast.skill.level, player.toBeCast.targetPosition);
                player.toBeCast.skill.remainingCooldown = player.toBeCast.skill.cooldown;
                player.toBeCast = null;
            } else if (player.getChannel() == null && casting > 0) {
                SkillPreset skill = null;
                if (casting == 1)
                    skill = player.skillSet.tryToCast(SkillType.fireball);
                else if (casting == 2)
                    skill = player.skillSet.tryToCast(SkillType.blink);
                if (skill == null) {
                    casting = 0;
                    return;
                }
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;

                if (playerPlane.Raycast(ray, out hitdist) && networkView.isMine)
                    targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                transform.rotation = targetRotation;
                player.addBuff(new Channel(player, skill, new Vector3(transform.position.x, 1, transform.position.z), targetRotation, targetPoint));
            }
        }
    }

    [RPC]
    public void initializeSkill(NetworkViewID id, int level, Vector3 targetPosition) {
        GameObject skillObject = NetworkView.Find(id).gameObject;
        SkillScript ss = skillObject.GetComponent<SkillScript>();
        ss.initialize();
        Skill skill = ss.skill;
        if (skill != null) {
            skill.level = level;
            skill.targetPosition = targetPosition;
        }
    }
}