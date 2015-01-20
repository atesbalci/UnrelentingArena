using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    public Blink(Player player)
        : base(player) {
        prefab = "Blink";
    }

    public override void update(GameObject gameObject, float delta, Vector3 targetPosition) {
        base.update(gameObject, delta, targetPosition);
        player.schedulePositionChange(gameObject.transform.position);
    }
}
