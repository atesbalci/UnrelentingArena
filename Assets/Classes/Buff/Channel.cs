﻿using UnityEngine;
using System.Collections;

public class Channel : Buff {
    public SkillPreset skill { get; set; }
    public Quaternion rotation { get; set; }
    public Vector3 position { get; set; }
    public Vector3 targetPosition { get; set; }
    public bool onRecoil { get; set; }

    public Channel(Player player, SkillPreset skill, Vector3 position, Quaternion rotation, Vector3 targetPosition)
        : base(player) {
        duration = skill.channelTime;
        this.skill = skill;
        this.rotation = rotation;
        this.position = position;
        this.targetPosition = targetPosition;
        onRecoil = false;
    }

    public override void update() {
        base.update();
    }

    public override void buff() {
        player.currentSpeed = 0;
    }

    public override void debuff() {
        if (!onRecoil) {
            player.addBuff(this);
            duration = skill.recoilTime;
            player.toBeCast = this;
            onRecoil = true;
        } else {
            player.currentSpeed = player.movementSpeed;
            player.movementReset = true;
        }
    }
}
