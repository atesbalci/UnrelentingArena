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
        if (GUIUtility.hotControl == 0) {
            if (controlScript.spell1) {
                casting = 1;
            } else if (controlScript.spell2) {
                casting = 2;
            }
        }

        if (player.toBeCast != null) {
            Skill toBeCast = null;
            if (player.toBeCast.skill.skill == SkillSet.FIREBALL) {
                toBeCast = new Fireball();
            } else if (player.toBeCast.skill.skill == SkillSet.BLINK) {
                toBeCast = new Blink();
            }
            toBeCast.range = player.toBeCast.skill.range;
            toBeCast.damage = player.toBeCast.skill.damage;
            toBeCast.player = player;
            toBeCast.targetPosition = player.toBeCast.targetPosition;
            instantiateSkill(toBeCast, player.toBeCast.position, player.toBeCast.rotation);
            player.toBeCast.skill.remainingCooldown = player.toBeCast.skill.cooldown;
            player.toBeCast = null;
        } else if (player.getChannel() == null && casting > 0) {
            SkillPreset skill = null;
            if (casting == 1)
                skill = player.skillSet.tryToCast(SkillSet.FIREBALL);
            else if (casting == 2)
                skill = player.skillSet.tryToCast(SkillSet.BLINK);
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

    public void instantiateSkill(Skill skill, Vector3 position, Quaternion rotation) {
        GameObject skillObject = MonoBehaviour.Instantiate(Resources.Load(skill.prefab), position, rotation) as GameObject;
        SkillScript skillScript = skillObject.GetComponent<SkillScript>();
        skillScript.skill = skill;
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        Vector3 pos = new Vector3(0, 0, 0);
        int cas = casting;
        float rd = -1;
        Channel c = player.getChannel();

        if (stream.isWriting) {
            pos = targetPoint;
            cas = casting;
            if (c != null)
                rd = c.remainingDuration;
            stream.Serialize(ref pos);
            stream.Serialize(ref cas);
            stream.Serialize(ref rd);
        } else {
            stream.Serialize(ref pos);
            stream.Serialize(ref cas);
            stream.Serialize(ref rd);
            if (c != null)
                c.remainingDuration = rd;
            targetPoint = pos;
            if (cas != 0)
                casting = cas;
        }
    }
}