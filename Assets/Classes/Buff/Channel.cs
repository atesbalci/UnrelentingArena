using UnityEngine;
using System.Collections;

public class Channel : Buff {
    public Skill skill { get; set; }
    public Quaternion rotation { get; set; }
    public Vector3 position { get; set; }

    public Channel(Player player, Skill skill, Vector3 position, Quaternion rotation)
        : base(player) {
        duration = skill.channelTime;
        this.skill = skill;
        this.rotation = rotation;
        this.position = position;
    }

    public override void update() {
        base.update();
    }

    public override void buff() {
        player.currentSpeed = 0;
    }

    public override void debuff() {
        if (skill != null) {
            player.addBuff(this);
            GameObject skillGameObject = MonoBehaviour.Instantiate(Resources.Load(skill.prefab, typeof(GameObject)), position, rotation) as GameObject;
            SkillScript skillScript = skillGameObject.AddComponent<SkillScript>();
            skillScript.skill = skill;
            duration = skill.recoilTime;
            skill = null;
        } else {
            player.currentSpeed = player.movementSpeed;
            player.movementReset = true;
        }
    }
}
