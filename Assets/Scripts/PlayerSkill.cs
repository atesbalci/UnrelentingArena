﻿using UnityEngine;
using System.Collections;

public class PlayerSkill : MonoBehaviour {
    private Player player;
    private Vector3 targetPoint;
    private int casting;
    private Animator anim;
    public NetworkView view;
    private PlayerMove playerMove;

    void Start() {
        player = GetComponent<PlayerScript>().player;
        casting = -1;
        anim = GetComponent<Animator>();
        playerMove = GetComponent<PlayerMove>();
    }

    void Update() {
        if (!player.dead) {
            casting = -1;
            if (view.isMine && player.canCast) {
                for(int i = 0; i < 4; i++) {
                    if (Input.GetKeyDown(GameInput.instance.keys[(int)GameBinding.Skill1 + i])) {
                        casting = i;
                        break;
                    }
                }
            }

            if (player.toBeCast != null) {
                //networkView.RPC("InstantiateSkill", RPCMode.All, player.toBeCast.skill.prefab, player.toBeCast.position,
                //    player.toBeCast.rotation, player.toBeCast.skill.level, player.toBeCast.targetPosition);
                InstantiateSkill(player.toBeCast.skill.name, player.toBeCast.position,
                    player.toBeCast.rotation, player.toBeCast.skill.level, player.toBeCast.targetPosition);
                player.toBeCast.skill.remainingCooldown = player.toBeCast.skill.cooldown;
                player.toBeCast = null;
            } else if (player.channel == null && casting > -1) {
                SkillPreset skill = null;
                skill = player.skillSet.TryToCast(casting);
                if (skill == null) {
                    casting = -1;
                    return;
                }
                Plane playerPlane = new Plane(Vector3.up, transform.position);
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                float hitdist = 0.0f;
                if (playerPlane.Raycast(ray, out hitdist) && view.isMine)
                    targetPoint = ray.GetPoint(hitdist);
                Quaternion targetRotation = Quaternion.LookRotation(targetPoint - transform.position);
                playerMove.destinationPosition = Vector3.Lerp(transform.position, targetPoint, 0.05f);
                player.AddBuff(new Channel(player, skill, new Vector3(transform.position.x, 1, transform.position.z), targetRotation, targetPoint));
            }
        }
    }

    public void InstantiateSkill(string prefab, Vector3 position, Quaternion rotation, int level, Vector3 targetPosition) {
        GameObject skillObject = Network.Instantiate(Resources.Load(prefab), position, rotation, 0) as GameObject;
        view.RPC("InitializeSkill", RPCMode.All, skillObject.GetComponent<NetworkView>().viewID, level, targetPosition);
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
        anim.SetBool("Casting", false);
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (stream.isWriting) {
            bool casting = anim.GetBool("Casting");
            stream.Serialize(ref casting);
        } else {
            bool casting = false;
            stream.Serialize(ref casting);
            anim.SetBool("Casting", casting);
        }
    }
}