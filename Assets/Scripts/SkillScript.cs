﻿using UnityEngine;
using System.Collections;

public class SkillScript : MonoBehaviour {
    public SkillType skillType;
    public NetworkView view;
    public Skill skill { get; set; }

    private bool ended;

    void Start() {
        ended = false;
        Initialize();
        if (skill != null)
            skill.Start(gameObject);
    }

    public void Initialize() {
        if (skill == null) {
            switch (skillType) {
                case SkillType.Powerball:
                    skill = new Powerball();
                    break;
                case SkillType.Blink:
                    skill = new Blink();
                    break;
                case SkillType.Meteor:
                    skill = new Meteor();
                    break;
                case SkillType.Overcharge:
                    skill = new Overcharge();
                    break;
                case SkillType.Orb:
                    skill = new Orb();
                    break;
                case SkillType.Charge:
                    skill = new Charge();
                    break;
                case SkillType.Boomerang:
                    skill = new Boomerang();
                    break;
                case SkillType.Mine:
                    skill = new Mine();
                    break;
            }
        }
    }

    void Update() {
        if (skill != null) {
            if (!skill.dead)
                skill.Update();
            else
                skill.UpdateEnd();
        }
        if (Network.isServer && !ended && skill.dead)
            view.RPC("End", RPCMode.All);
    }

    void OnTriggerStay(Collider collider) {
        if (skill != null && Network.isServer && !skill.dead) {
            SkillScript skillScript = collider.gameObject.GetComponent<SkillScript>();
            PlayerScript playerScript = collider.gameObject.GetComponent<PlayerScript>();
            if (playerScript != null) {
                if (skill.player != playerScript.player)
                    skill.CollisionWithPlayer(collider, playerScript.player);
                else
                    skill.CollisionWithSelf(collider);
            } else if (skillScript != null)
                skill.CollisionWithSkill(collider, skillScript.skill);
            else
                skill.CollisionWithOtherObject(collider);
        }
    }

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
        if (skill != null)
            skill.SerializeNetworkView(stream, info);
    }

    [RPC]
    private void End() {
        skill.dead = true;
        ended = true;
    }
}
