using UnityEngine;
using System.Collections;

public class CastChannel : Stun {
    public SkillPreset skill { get; set; }
    public Quaternion rotation { get; set; }
    public Vector3 position { get; set; }
    public Vector3 targetPosition { get; set; }
    public float recoilTime { get; set; }

    public CastChannel(Player player, SkillPreset skill, Vector3 position, Quaternion rotation, Vector3 targetPosition)
        : base(player, skill.channelTime) {
        duration = skill.channelTime;
        this.skill = skill;
        this.rotation = rotation;
        this.position = position;
        this.targetPosition = targetPosition;
        recoilTime = skill.recoilTime;
    }

    public override void ApplyBuff() {
    }

    public override void Unbuff() {
        player.toBeCast = this;
        player.AddBuff(new CastRecoil(player, recoilTime));
    }
}
