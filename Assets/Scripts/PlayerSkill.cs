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
        if (player.canCast) {
            if (controlScript.spell1) {
                casting = 1;
            } else if (controlScript.spell2) {
                casting = 2;
            }
        }

        if (player.toBeCast != null) {
            //networkView.RPC("InstantiateSkill", RPCMode.All, player.toBeCast.skill.prefab, player.toBeCast.position,
            //    player.toBeCast.rotation, player.toBeCast.skill.level, player.toBeCast.targetPosition);
            InstantiateSkill(player.toBeCast.skill.prefab, player.toBeCast.position,
                player.toBeCast.rotation, player.toBeCast.skill.level, player.toBeCast.targetPosition);
            player.toBeCast.skill.remainingCooldown = player.toBeCast.skill.cooldown;
            player.toBeCast = null;
        } else if (player.Channel == null && casting > 0) {
            SkillPreset skill = null;
            if (casting == 1)
                skill = player.skillSet.TryToCast(SkillType.Fireball);
            else if (casting == 2)
                skill = player.skillSet.TryToCast(SkillType.Blink);
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
            GetComponent<PlayerMove>().networkView.RPC("Move", RPCMode.All, transform.position, targetRotation);
            player.AddBuff(new Channel(player, skill, new Vector3(transform.position.x, 1, transform.position.z), targetRotation, targetPoint));
        }
    }

    [RPC]
    public void InstantiateSkill(string prefab, Vector3 position, Quaternion rotation, int level, Vector3 targetPosition) {
        if (true) {
            GameObject skillObject = Network.Instantiate(Resources.Load(prefab), position, rotation, 0) as GameObject;
            networkView.RPC("InitializeSkill", RPCMode.AllBuffered, skillObject.networkView.viewID, level, targetPosition);
        }
    }

    [RPC]
    public void InitializeSkill(NetworkViewID id, int level, Vector3 targetPosition) {
        GameObject skillObject = NetworkView.Find(id).gameObject;
        SkillScript skillScript = skillObject.GetComponent<SkillScript>();
        skillScript.Initialize();
        Skill skill = skillScript.skill;
        if (skill != null) {
            skill.level = level;
            skill.targetPosition = targetPosition;
            skill.player = player;
        }
    }
}