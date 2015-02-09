using UnityEngine;
using System.Collections;

public class Channel : Stun {
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

    public override void Unbuff() {
        if (!onRecoil) {
            player.AddBuff(this);
            duration = skill.recoilTime;
            player.toBeCast = this;
            onRecoil = true;
        } else
            base.Unbuff();
    }
}
