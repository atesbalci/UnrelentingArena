using UnityEngine;
using System.Collections;

public class Blink : TargetSkill {
    private float timeBeforeDestruction;
    private bool blinkDone;

    public Blink(Player player)
        : base(player) {
        prefab = "Blink";
        blinkDone = false;
        timeBeforeDestruction = 3;
    }

    public override void update(GameObject gameObject, float delta, Vector3 targetPosition) {
        base.update(gameObject, delta, targetPosition);
        if (!blinkDone) {
            player.schedulePositionChange(gameObject.transform.position);
            blinkDone = true;
        } else {
            timeBeforeDestruction -= delta;
            if (timeBeforeDestruction <= 0)
                MonoBehaviour.Destroy(gameObject);
        }
    }
}
